using System;
using System.Net;
using System.Threading;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;

namespace Metreos.Providers.PCapService
{
    /// <summary>
    /// Provides packet capturing services.
    /// </summary>
    [ProviderDecl("PCapService Provider")]
    [PackageDecl("Metreos.Providers.PCapService", "Suite of actions and events for packet capturing service")]
    public class PCapServiceProvider : ProviderBase
    {
        #region Constants
        private abstract class Consts
        {
            public abstract class ConfigEntries
            {
                public const string PCapPortbaseLWM      = "PCapPortbaseLWM";
                public const string PCapPortbaseHWM      = "PCapPortbaseHWM";
                public const string PCapPortStep         = "PCapPortStep";
                public const string PCapMaxMonitorTime   = "PCapMaxMonitorTime";
                public const string PCapLogLevel         = "PCapLogLevel";
            }

            public abstract class DefaultValues
            {
                public const int PCapPortbaseLWM                    = 25000;
                public const int PCapPortbaseHWM                    = 26000;
                public const int PCapPortStep                       = 2;
                public const int PCapMaxMonitorTime                 = 480;      // 480 minutes = 8 hours
                public const int PCapLogLevel                       = 2;        // Warning
                public const int DefaultTransactionCleanerInterval  = 30;       // 30 minutes
            }

            public const string DisplayName         = "Packet Capture Provider";

            public const string PCAPHOST            = "127.0.0.1";
            public const int PCAPIPCPORT            = 8510;
        }
        #endregion // Constants

        #region Transactions
        private class Transaction
        {
            public Transaction(ActionBase ab) { action = ab; isDone = false; } 
            public bool isDone;
            public ActionBase action;
        }
        #endregion // Transactions

        private PCapServiceIpcClient pCapClient;
        private Hashtable requestTransactions;
        private Timer transactionCleanerTimer;
        private bool isActive;

        private volatile static uint transactionId       = 0;

        #region Constructor

        public PCapServiceProvider(IConfigUtility configUtility) 
            : base(typeof(PCapServiceProvider), Consts.DisplayName, configUtility)
        {
            isActive = false;
            pCapClient = new PCapServiceIpcClient();
            pCapClient.Log = log;

            pCapClient.onActiveCallList += new OnActiveCallListDelegate(OnCallListAck);
            pCapClient.onMonitorCallRequestAck += new OnMonitorCallRequestAckDelegate(OnMonitorCallAck);

            requestTransactions = Hashtable.Synchronized(new Hashtable());
        }

        #endregion // Constructor

        #region ProviderBase Methods
        protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            this.messageCallbacks.Add(IPCapService.Actions.CALL_LIST, new HandleMessageDelegate(this.HandleCallList));
            this.messageCallbacks.Add(IPCapService.Actions.MONITOR_CALL, new HandleMessageDelegate(this.HandleMonitorCall));
            this.messageCallbacks.Add(IPCapService.Actions.STOP_MONITOR_CALL, new HandleMessageDelegate(this.HandleStopMonitorCall));

            configItems = new ConfigEntry[5];

            configItems[0] = new ConfigEntry(Consts.ConfigEntries.PCapPortbaseLWM, "Port Range (min)", 
                Consts.DefaultValues.PCapPortbaseLWM, "Lower-bound of RTP port range", 25000, 30000, true);
            configItems[1] = new ConfigEntry(Consts.ConfigEntries.PCapPortbaseHWM, "Port Range (max)", 
                Consts.DefaultValues.PCapPortbaseHWM, 
                "Upper-bound of RTP port range. (Must be greater than lower-bound)", 25000, 30000, true);
            configItems[2] = new ConfigEntry(Consts.ConfigEntries.PCapPortStep, "Port Increment", 
                Consts.DefaultValues.PCapPortStep, "Port number increments for new RTP stream", 2, 10, true);
            configItems[3] = new ConfigEntry(Consts.ConfigEntries.PCapMaxMonitorTime, "Max Monitor Time", 
                Consts.DefaultValues.PCapMaxMonitorTime, 
                "Maximum number of minutes to monitor a phone call", 1, 1000, true);
            configItems[4] = new ConfigEntry(Consts.ConfigEntries.PCapLogLevel, "PCap Log Level", 
                Consts.DefaultValues.PCapLogLevel, 
                "Log level for Packet Capturing Service", 0, 4, true);

            // No extensions
            extensions = null;

            return true;
        }

        protected override void RefreshConfiguration()
        {
            // Send new configuration to pcap-service
            pCapClient.PortbaseLWM = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.PCapPortbaseLWM));
            pCapClient.PortbaseHWM = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.PCapPortbaseHWM));
            pCapClient.PortStep = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.PCapPortStep));
            pCapClient.MaxMonitorTime = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.PCapMaxMonitorTime));
            pCapClient.LogLevel = Convert.ToUInt32(GetConfigValue(Consts.ConfigEntries.PCapLogLevel));

            // This method will be called if the provider is disabled.
            // Don't start the PCap client if we're just going to get torn down.
            if(base.componentInfo.status != IConfig.Status.Disabled)
                StartPCapClient();
        }

        public override void Cleanup()
        {
            base.Cleanup();
        }

        protected override void OnStartup()
        {
            this.RegisterNamespace();

            if(isActive)
                OnShutdown();
            
            lock(this)
            {
                StartPCapClient();
                transactionCleanerTimer = new Timer(new TimerCallback(TransactionCleaner), 
                    null,
                    new TimeSpan(0, 0, Consts.DefaultValues.DefaultTransactionCleanerInterval, 0, 0),
                    new TimeSpan(0, 0, Consts.DefaultValues.DefaultTransactionCleanerInterval, 0, 0));

                isActive = true;
            }
        }
        
        protected override void OnShutdown()
        {
            lock(this)
            {
                if (transactionCleanerTimer != null)
                    transactionCleanerTimer.Dispose();

                pCapClient.Close();

                requestTransactions.Clear();

                isActive = false;
            }
        }

        #endregion  //ProviderBase Methods

        #region Action Handlers
        [Action(IPCapService.Actions.CALL_LIST, false, "Active Calls", "Request list of active calls", true)]
        [ReturnValue()]
        private void HandleCallList(ActionBase actionBase)
        {
            bool success = true;

            uint transactionId = NextTransactionId();
            Transaction tr = new Transaction(actionBase);
            lock (requestTransactions.SyncRoot)
            {
                requestTransactions.Add(transactionId, tr);
            }

            success = pCapClient.SendRequestActiveCallListMessage(transactionId);

            actionBase.SendResponse(success);
        }

        [Action(IPCapService.Actions.MONITOR_CALL, false, "Monitor Call", "Request to monitor an active call", true)]
        [ActionParam(IPCapService.Fields.FIELD_CALL_IDENTIFIER, typeof(uint), true, false, "Call Identifier")]
        [ActionParam(IPCapService.Fields.FIELD_MMS_IP, typeof(string), true, false, "Media Server IP Address")]
        [ActionParam(IPCapService.Fields.FIELD_MMS_PORT1, typeof(uint), true, false, "Media Server RTP listening port #1")]
        [ActionParam(IPCapService.Fields.FIELD_MMS_PORT2, typeof(uint), true, false, "Media Server RTP listening port #2")]
        [ReturnValue()]
        private void HandleMonitorCall(ActionBase actionBase)
        {
            bool success = true;

            uint callIdentifier = Convert.ToUInt32(actionBase.InnerMessage[IPCapService.Fields.FIELD_CALL_IDENTIFIER]);
            string mmsIp = Convert.ToString(actionBase.InnerMessage[IPCapService.Fields.FIELD_MMS_IP]);
            uint mmsPort1 = Convert.ToUInt32(actionBase.InnerMessage[IPCapService.Fields.FIELD_MMS_PORT1]);
            uint mmsPort2 = Convert.ToUInt32(actionBase.InnerMessage[IPCapService.Fields.FIELD_MMS_PORT2]);

            uint transactionId = NextTransactionId();
            Transaction tr = new Transaction(actionBase);
            lock (requestTransactions.SyncRoot)
            {
                requestTransactions.Add(transactionId, tr);
            }
            
            success = pCapClient.SendMonitorCallRequestMessage(transactionId, callIdentifier, mmsIp, mmsPort1, mmsPort2);

            actionBase.SendResponse(success);
        }

        [Action(IPCapService.Actions.STOP_MONITOR_CALL, false, "Stop Monitor Call", "Request to stop monitoring an active call", false)]
        [ActionParam(IPCapService.Fields.FIELD_CALL_IDENTIFIER, typeof(uint), true, false, "Call Identifier")]
        [ReturnValue()]
        private void HandleStopMonitorCall(ActionBase actionBase)
        {
            bool success = true;

            uint callIdentifier = Convert.ToUInt32(actionBase.InnerMessage[IPCapService.Fields.FIELD_CALL_IDENTIFIER]);

            success = pCapClient.SendStopMonitorCallRequestMessage(callIdentifier);

            actionBase.SendResponse(success);
        }

        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
        {
            log.Write(TraceLevel.Info, "{0} event was not handled", originalEvent.MessageId); 
        }
        #endregion //Action Handlers

        #region Events
        private void OnCallListAck(uint transactionId, Hashtable calls)
        {
            Transaction tr;
            lock (requestTransactions.SyncRoot)
            {
                tr = requestTransactions[transactionId] as Transaction;
                tr.isDone = true;
            }

            AsyncAction action = tr.action as AsyncAction;
            if (action == null)
            {
                log.Write(TraceLevel.Warning, "Can not find action for pending request, transaction id = " + transactionId);
                return;
            }
          
            EventMessage eMsg = action.CreateAsyncCallback(true);

            eMsg.AddField(IPCapService.Fields.FIELD_ACTIVE_CALLS, calls);

            base.palWriter.PostMessage(eMsg);
        }

        private void OnMonitorCallAck(uint transactionId, MonitoredCallData data)
        {
            Transaction tr;
            lock (requestTransactions.SyncRoot)
            {
                tr = requestTransactions[transactionId] as Transaction;
                tr.isDone = true;
            }

            AsyncAction action = tr.action as AsyncAction;
            if (action == null)
            {
                log.Write(TraceLevel.Warning, "Can not find action for pending request, transaction id = " + transactionId);
                return;
            }

            bool rc = data.ResultCode == 0 ? true : false;      // 0 = true, 1 = false
            EventMessage eMsg = action.CreateAsyncCallback(rc);    
            eMsg.AddField(IPCapService.Fields.FIELD_RESULT_CODE, data.ResultCode);
            eMsg.AddField(IPCapService.Fields.FIELD_CALL_IDENTIFIER, data.CallIdentifier);
            eMsg.AddField(IPCapService.Fields.FIELD_RTP_PORT1, data.RtpPort1);
            eMsg.AddField(IPCapService.Fields.FIELD_RTP_PORT2, data.RtpPort2);
            eMsg.AddField(IPCapService.Fields.FIELD_CALL_TYPE, data.CallType);
            eMsg.AddField(IPCapService.Fields.FIELD_CALLER_DN, data.CallerDN);
            eMsg.AddField(IPCapService.Fields.FIELD_CALLEE_DN, data.CalleeDN);
            eMsg.AddField(IPCapService.Fields.FIELD_PHONE_IP, data.PhoneIp);

            log.Write(TraceLevel.Info, "Sending monitor call result for: " + data.CallIdentifier);
            
            base.palWriter.PostMessage(eMsg);
        }

        [Event(IPCapService.Actions.CALL_LIST, true)]
        [EventParam(IPCapService.Fields.FIELD_ACTIVE_CALLS, typeof(Hashtable), true, "Active Calls")]
        private void CallListSuccess() {}

        [Event(IPCapService.Actions.CALL_LIST, false)]
        private void CallListFailure() {}

        [Event(IPCapService.Actions.MONITOR_CALL, true)]
        [EventParam(IPCapService.Fields.FIELD_RESULT_CODE, typeof(uint), true, "Result Code")]
        [EventParam(IPCapService.Fields.FIELD_CALL_IDENTIFIER, typeof(uint), true, "Call Identifier")]
        [EventParam(IPCapService.Fields.FIELD_RTP_PORT1, typeof(uint), true, "RTP Port1")]
        [EventParam(IPCapService.Fields.FIELD_RTP_PORT2, typeof(uint), true, "RTP Port2")]
        [EventParam(IPCapService.Fields.FIELD_CALL_TYPE, typeof(uint), true, "Call Type")]
        [EventParam(IPCapService.Fields.FIELD_CALLER_DN, typeof(string), true, "Caller DN")]
        [EventParam(IPCapService.Fields.FIELD_CALLEE_DN, typeof(string), true, "Callee DN")]
        [EventParam(IPCapService.Fields.FIELD_PHONE_IP, typeof(string), true, "Monitored Phone IP")]
        private void MonitorCallSuccess() {}

        [Event(IPCapService.Actions.MONITOR_CALL, false)]
        private void MonitorCallFailure() {}
        #endregion // Events

        #region Helper methods
        private void StartPCapClient()
        {
            if (pCapClient.State == IPCState.Disconnected)
            {
                IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(Consts.PCAPHOST), Consts.PCAPIPCPORT); 
                pCapClient.Startup(ipe);
            }
            else if (pCapClient.State == IPCState.Connected)
            {
                pCapClient.SendConfigData();
            }
        }

        private static uint NextTransactionId()
        {
            return transactionId++;
        }

        private void TransactionCleaner(object state)
        {
            log.Write(TraceLevel.Verbose, "Transaction Cleaner starts");

            Transaction tr;
            ArrayList completedTransactions = new ArrayList();
            lock (requestTransactions.SyncRoot)
            {
                IDictionaryEnumerator i = requestTransactions.GetEnumerator();
                while(i.MoveNext())
                {
                    tr = i.Value as Transaction;
                    if(tr != null)
                    {
                        if(tr.isDone)
                            completedTransactions.Add(i.Key);
                    }
                }

            }

            foreach(object transactionId in completedTransactions)
            {
                requestTransactions.Remove(transactionId);
            }

            completedTransactions.Clear();
            
        }
        #endregion // Helper methods
    }
}
