using System;
using System.Diagnostics;

using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

using Metreos.Configuration;

namespace Metreos.AppServer.Clustering
{
	/// <summary>Handles fault-tolerance and failover logic</summary>
	internal class FailoverManager
	{
        private readonly LogWriter log;

        // Messaging
        private readonly MessageUtility msgUtility;
        private MessageQueueWriter routerQ;

		internal FailoverManager(LogWriter log, MessageQueue clusterQ)
		{
            this.log = log;

            this.msgUtility = new MessageUtility(IConfig.CoreComponentNames.CLUSTER_INTERFACE,
                IConfig.ComponentType.Core, clusterQ.GetWriter());
		}

        internal void Startup()
        {
            this.routerQ = MessageQueueFactory.GetQueueWriter(IConfig.CoreComponentNames.ROUTER);
            if(routerQ == null)
                throw new Metreos.Core.StartupFailedException("Failed to acquire queue writer for Router");
        }

        #region Parent Behaviors

        internal void SetParentStatus(IConfig.FailoverStatus sbStatus)
        {
            if(Config.ParentFailoverStatus != sbStatus)
            {
                log.Write(TraceLevel.Info, "Failover status changed to: {0}", 
                    sbStatus.ToString());

                // If the standby has finished relinquishing our devices, refresh providers.
                if(Config.ParentFailoverStatus == IConfig.FailoverStatus.Failback &&
                    sbStatus == IConfig.FailoverStatus.Normal)
                {
                    Config.ParentFailoverStatus = IConfig.FailoverStatus.Normal;
                    RefreshProviders();
                }
                else
                {
                    Config.ParentFailoverStatus = sbStatus;
                }
            }
        }
        #endregion

        #region Standby Behaviors

        internal void HandleParentConnectionFailure()
        {
            Config.StandbyFailoverStatus = IConfig.FailoverStatus.Failover;

            log.Write(TraceLevel.Info, "Failover status: Parent={0}, Standby={1}",
                Config.ParentFailoverStatus.ToString(), Config.StandbyFailoverStatus.ToString());

            RefreshProviders();
        }

        internal void HandleParentConnectionRestored()
        {
            if(Config.StandbyFailoverStatus == IConfig.FailoverStatus.Failover)
            {
                Config.StandbyFailoverStatus = IConfig.FailoverStatus.Failback;

                log.Write(TraceLevel.Info, "Failover status: Parent={0}, Standby={1}",
                    Config.ParentFailoverStatus.ToString(), Config.StandbyFailoverStatus.ToString());

                RefreshProviders();
            }
        }

        #endregion

        #region Message handlers

        internal void HandleRefreshResponse()
        {
            log.Write(TraceLevel.Info, "Providers have completed failover actions");

            if(Config.StandbyFailoverStatus == IConfig.FailoverStatus.Failback)
            {
                Config.StandbyFailoverStatus = IConfig.FailoverStatus.Normal;
            }
        }

        internal string GetDiagMsg()
        {
            return string.Format("Failover status as parent : {0}\r\nFailover status as standby: {1}",
                Config.ParentFailoverStatus.ToString(), Config.StandbyFailoverStatus.ToString());
        }
        #endregion

        #region Helpers

        private void RefreshProviders()
        {
            // Tell ProviderManager to send RefreshConfig messages to providers
            CommandMessage cMsg = msgUtility.CreateCommandMessage(IConfig.CoreComponentNames.PROV_MANAGER,
                ICommands.REFRESH_PROVIDERS);
            routerQ.PostMessage(cMsg);
        }
    
        #endregion
	}
}
