using System;
using System.Diagnostics;
using System.Collections.Specialized;

using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.LoggingFramework;

using Metreos.Configuration;
using Metreos.AppServer.ProviderManager.Collections;

namespace Metreos.AppServer.ProviderManager
{
	/// <summary>
	/// Handles startup and shutdown of providers
	/// </summary>
	internal sealed class ProviderInitManager : IDisposable
	{
        private const string Name = "ProviderInitManager";
        
        private LogWriter log;
        private Config config;
        private MessageQueue msgQ;
        private MessageUtility messageUtility;

		public ProviderInitManager(LogWriter log)
		{
            this.log = log;
            this.config = Config.Instance;
            this.msgQ = new MessageQueue(Name, config.MessageQueueProvider, log);
            this.messageUtility = new MessageUtility(Name, IConfig.ComponentType.Core, msgQ.GetWriter());
		}

        public bool Startup(ProviderInfo pInfo, uint timeout)
        {
            Assertion.Check(msgQ != null, "Message queue is null");

            msgQ.Purge();

            if(pInfo.ProviderQ == null)
            {
                log.Write(TraceLevel.Error, "Could not obtain an IPC queue writer for: " + pInfo.Name);
                return false;
            }

            // Send startup message
            pInfo.ProviderQ.PostMessage(messageUtility.CreateCommandMessage(pInfo.Name, ICommands.STARTUP));

            DateTime end = DateTime.Now.AddMilliseconds(timeout);

            // Wait for response
            while(DateTime.Now < end && !pInfo.StartupAborted)
            {
                InternalMessage msg;
                msgQ.Receive(System.TimeSpan.FromMilliseconds(20), out msg);

                if(msg != null)
                {
                    ResponseMessage respMsg = msg as ResponseMessage;
                    if(respMsg != null && respMsg.MessageId == IResponses.STARTUP_COMPLETE)
                    {
                        log.Write(TraceLevel.Info, "Provider started: " + pInfo.Name);
                        return true;
                    }
                }
            }

            log.Write(TraceLevel.Warning, "Provider failed to startup: " + pInfo.Name);
            return false;
        }

        public bool Shutdown(ProviderInfo pInfo, uint timeout)
        {
            Assertion.Check(msgQ != null, "Message queue is null");

            msgQ.Purge();

            if(pInfo.ProviderQ == null || pInfo.Status != IConfig.Status.Enabled_Running)
                return true;

            // Send shutdown message
            pInfo.ProviderQ.PostMessage(messageUtility.CreateCommandMessage(pInfo.Name, ICommands.SHUTDOWN));
            
            DateTime end = DateTime.Now.AddMilliseconds(timeout);

            // Wait for response
            while(DateTime.Now < end)
            {
                InternalMessage msg;
                msgQ.Receive(System.TimeSpan.FromMilliseconds(20), out msg);

                if(msg != null)
                {
                    ResponseMessage respMsg = msg as ResponseMessage;
                    if(respMsg != null && respMsg.MessageId == IResponses.SHUTDOWN_COMPLETE)
                    {
                        log.Write(TraceLevel.Info, "Provider shutdown gracefully: " + pInfo.Name);
                        return true;
                    }
                }
            }

            log.Write(TraceLevel.Warning, "Provider failed to shutdown gracefully: " + pInfo.Name);
            return false;
        }

        public void Dispose()
        {
            if(msgQ != null)
            {
                msgQ.Dispose();
                msgQ = null;
            }
        }
	}
}
