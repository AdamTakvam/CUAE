using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

using Metreos.Core;
using Metreos.Core.ConfigData;
using Metreos.Messaging;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Interfaces;
using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;

using Package = Metreos.Interfaces.PackageDefinitions.Http;

namespace Metreos.Providers.Http
{
	[ProviderDecl(Package.Globals.DISPLAY_NAME)]
	[PackageDecl(Package.Globals.NAMESPACE, Package.Globals.PACKAGE_DESCRIPTION)]
	public class HTTP : ProviderBase
	{
		// Constants
        private abstract class Consts
        {
            public const int Port                           = 8000;
            public const int ModPort			            = 9434;
            public static TimeSpan SessionExpiration        = new TimeSpan(0, 20, 0);
            public static TimeSpan SessionCleanupInterval   = new TimeSpan(0, 1, 0);
        }

#if INTERNAL_STACK
		/// <summary>Port to listen on for incoming HTTP requests.</summary>
		public int port;
#else
		/// <summary>Port to listen on for incoming HTTP requests from Apache module through IPC.</summary>
		public int modPort = Consts.ModPort;
#endif

		/// <summary>Time before HTTP sessions expire.</summary>
		private TimeSpan sessionExpiration;

		/// <summary>Hash of active sessions to the HTTP provider.
		/// routingGuid (string) -> SessionData (object)</summary>
		private Hashtable sessions;

		/// <summary>Timer used to periodically clean up the active
		/// session table.</summary>
		private readonly TimerHandle sessionCleaner;

        /// <summary>Creates timers</summary>
        private readonly TimerManager timerManager;

#if INTERNAL_STACK
		/// <summary>The HTTP stack used by the provider.</summary>
		private HttpStack stack = null;
#else
		/// <summary>The HTTP listener for Apache module data.</summary>
		private HttpListener listener = null;
#endif

		/// <summary>
		/// Constructor for HTTP provider
		/// </summary>
		public HTTP(IConfigUtility configUtility)
            : base(typeof(HTTP), Package.Globals.DISPLAY_NAME, configUtility)
		{
			this.sessions = new Hashtable();
            this.timerManager = new TimerManager("HTTP Timers", new WakeupDelegate(SessionCleaner), 
                new WakeupExceptionDelegate(SessionCleanerException), 1, 1);
            this.sessionCleaner = timerManager.Add(Convert.ToInt64(Consts.SessionCleanupInterval.TotalMilliseconds));
		}

        #region Provider interface
		protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
		{
			// Add delegate(s) to hash of message handlers
			messageCallbacks.Add(Package.Actions.SendResponse.FULLNAME, 
				new HandleMessageDelegate(this.HandleSendResponse));

            messageCallbacks.Add(Package.Actions.SessionEnd.FULLNAME, 
                new HandleMessageDelegate(this.HandleSessionEnd));

			// Set default config values
#if INTERNAL_STACK
                configItems = new ConfigEntry[2];
                configItems[0] = new ConfigEntry("Port", "Port", Consts.Port, "Listen port", 1024, 65536, true);
                configItems[1] = new ConfigEntry("SessionExpirationMinutes", "Session Expiration Minutes", Consts.SessionExpiration.Minutes, "Number of minutes before HTTP sessions expire.", 1, 3600, true);
#else
                configItems = new ConfigEntry[1];
                configItems[0] = new ConfigEntry("SessionExpirationMinutes", "Session Expiration Minutes", Consts.SessionExpiration.Minutes, "Number of minutes before HTTP sessions expire.", 1, 3600, true);
#endif

            // No extensions
            extensions = null;

			return true;
		}

		protected override void RefreshConfiguration()
		{
#if INTERNAL_STACK
			int oldPort = port;
			try { port = (int)GetConfigValue("Port"); }
			catch
			{
				log.Write(TraceLevel.Warning, 
					"Invalid HTTP port specified in global config. Using port {0}.", Consts.Port);
				port = Consts.Port;
			}

            if(oldPort != port)
				RefreshStack();
#endif

            try { sessionExpiration = new TimeSpan(0, (int)GetConfigValue("SessionExpirationMinutes"), 0); }
			catch
			{
				log.Write(TraceLevel.Warning,
					"Invalid session expiration value specified in global config. Using {0} minutes.", Consts.SessionExpiration.Minutes);
				sessionExpiration = Consts.SessionExpiration;
			}
		}

		public override void Cleanup()
		{
			StopStack();

			this.sessionCleaner.Cancel();
            this.timerManager.Shutdown();

			base.Cleanup();
		}

		protected override void OnStartup()
		{
			this.RegisterNamespace();

			// Start the HTTP stack.
			StartStack();
		}

		protected override void OnShutdown()
		{
			StopStack();
		}
		#endregion
        
		#region Session Management

		/// <summary>Executes at regular intervals to clean up
		/// the session table.</summary>
		/// <remarks>Sessions will be expired and removed from
		/// the active sessions table. When a session is expired
		/// an event will be generated and sent to the script
		/// instance triggered by the initial request.</remarks>
		/// <param name="state">Not used.</param>
		private long SessionCleaner(TimerHandle timer, object state)
		{
            log.Write(TraceLevel.Verbose, 
                "Session cleanup starting: {0} active sessions.", 
                sessions.Count);

            int numExpiredSessions = 0;
            DateTime start = DateTime.Now;

            Hashtable expiredSessions = null;

            lock(sessions.SyncRoot)
            {
                expiredSessions = new Hashtable(sessions);  // ref copy
            }

			foreach(DictionaryEntry de in expiredSessions)
			{
                string sessionKey = de.Key as string;
                SessionData session = de.Value as SessionData;

                if(sessionKey != null && session != null && session.IsExpired)
                {
                    SendExpiredSessionEvent(sessionKey);
                    numExpiredSessions++;
                    Thread.Sleep(5);  // be gentle
                }

                // SMA-714, Instead of removing expired sessions here, wait
                // until we get a NoHandler or SessionEnd action to remove expired session.
            }

			TimeSpan totalTime = DateTime.Now - start;

			log.Write(TraceLevel.Verbose, 
				"Session cleanup completed in {0}ms: {1} sessions expired.", 
				totalTime.TotalMilliseconds, numExpiredSessions);

            return Convert.ToInt64(Consts.SessionCleanupInterval.TotalMilliseconds);
		}

        private void SessionCleanerException(TimerHandle timer, object state, Exception e)
        {
            log.Write(TraceLevel.Warning, "HTTP Session cleaner caught exception: " + e.Message);
        }

		/// <summary>Creates a new HTTP session and adds it to
		/// the active sessions table.</summary>
		/// <param name="guid">The routing GUID of the script
		/// instance associated with the new sessions.</param>
		private void CreateNewHttpSession(string guid)
		{
			if(guid == null) return;

            lock(sessions.SyncRoot)
            {
                SessionData s = new SessionData(sessionExpiration.Minutes); 
                sessions[guid] = s;
                log.Write(TraceLevel.Verbose, "Session added, GUID=" + guid);
            }
		}


		/// <summary>Remove an HTTP session from the table of 
		/// active sessions.</summary>
		/// <param name="guid">The routing GUID of the session to
		/// remove.</param>
		private void RemoveHttpSession(string guid)
		{
			if(guid == null) return;

            lock(sessions.SyncRoot)
            {
                sessions.Remove(guid);
                log.Write(TraceLevel.Verbose, "Session removed, GUID=" + guid);
            }
		}

		#endregion

        #region HTTP Stack/Listener Callbacks

		/// <summary>Callback that is invoked by the HTTP stack when an incoming
		/// request is received off the wire.</summary>
		/// <param name="hMsg">The HttpMessage that was received.</param>
		/// <param name="guid">The routing GUID.</param>
		/// <param name="remoteHost">The host sending this request.</param>
        [Event(Package.Events.GotRequest.FULLNAME, eventTypeType.hybrid, Package.Actions.SendResponse.FULLNAME, Package.Events.GotRequest.DISPLAY, Package.Events.GotRequest.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.url.NAME, Package.Events.GotRequest.Params.url.DISPLAY, typeof(string), true, Package.Events.GotRequest.Params.url.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.remoteHost.NAME, Package.Events.GotRequest.Params.remoteHost.DISPLAY, typeof(string), true, Package.Events.GotRequest.Params.remoteHost.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.remoteIpAddress.NAME, Package.Events.GotRequest.Params.remoteIpAddress.DISPLAY, typeof(string), true, Package.Events.GotRequest.Params.remoteIpAddress.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.host.NAME, Package.Events.GotRequest.Params.host.DISPLAY, typeof(string), true, Package.Events.GotRequest.Params.host.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.hostname.NAME, Package.Events.GotRequest.Params.hostname.DISPLAY, typeof(string), true, Package.Events.GotRequest.Params.hostname.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.port.NAME, Package.Events.GotRequest.Params.port.DISPLAY, typeof(int), true, Package.Events.GotRequest.Params.port.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.query.NAME, Package.Events.GotRequest.Params.query.DISPLAY, typeof(string), true, Package.Events.GotRequest.Params.query.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.method.NAME, Package.Events.GotRequest.Params.method.DISPLAY, typeof(string), true, Package.Events.GotRequest.Params.method.DESCRIPTION)]
        [EventParam(Package.Events.GotRequest.Params.body.NAME, Package.Events.GotRequest.Params.body.DISPLAY, typeof(string), false, Package.Events.GotRequest.Params.body.DESCRIPTION)]
		private void HandleIncomingRequestCallback(HttpMessage hMsg, string guid, string remoteHost, EventMessage.EventType eventType)
		{
			DebugLog.MethodEnter();

            EventMessage msg = CreateEventMessage(Package.Events.GotRequest.FULLNAME, eventType, guid);
			msg = hMsg.PopulateInternalMessage(msg) as EventMessage;

			if(msg != null)
			{
				// Firefox makes a second HTTP request on its own violition, for a custom icon
				// that usually shows up next to the URL at the top of the browser.  This causes
				// an annoying second no handler to show up.  So, let's squish that error.
                string url = msg[Package.Events.GotRequest.Params.url.NAME] as string;
				if(url == "/favicon.ico")
				{
					HttpMessage notFoundMsg = new HttpMessage();
					notFoundMsg.uniqueId = hMsg.uniqueId;
					notFoundMsg.responseCode = "404";
					notFoundMsg.type = HttpMessage.Type.Response;
					
					SendResponse(notFoundMsg, remoteHost);

					DebugLog.MethodExit();
					return;
				}

				string remoteIp = remoteHost.Split(new char[] {':'}, 2)[0];

                msg.AddField(Package.Events.GotRequest.Params.remoteHost.NAME, remoteHost);
                msg.AddField(Package.Events.GotRequest.Params.remoteIpAddress.NAME, remoteIp);

				lock(sessions.SyncRoot)
				{
					if(sessions.Contains(guid))
					{
						SessionData s = sessions[guid] as SessionData;
						if(s != null)
						{
							s.ResetExpirationTime();
						}
					}
				}

				// Send it
				palWriter.PostMessage(msg);
			}

			DebugLog.MethodExit();
		}

		#endregion

        #region No handler
		protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
		{
            // Only notify when session expired.
            if (originalEvent.MessageId == Package.Events.SessionExpired.FULLNAME)
            {
                log.Write(TraceLevel.Verbose, "HandleNoHandler for expired session, GUID={0}", noHandlerAction.RoutingGuid);
                // Remove it from active sessions
                RemoveHttpSession(noHandlerAction.RoutingGuid);
                return;
            }

            // HTTP Provider will resend a non-triggering non-handled event as a triggering event, 
            // to give requests flagged incorrectly as session bound a second chance to trigger as a triggering event
            if(originalEvent.Type == EventMessage.EventType.NonTriggering)
            {
                // Correct HttpListener inner transaction tables 
                string newGuid = listener.CorrectQueueMapping(originalEvent.RoutingGuid);
                originalEvent.Type = EventMessage.EventType.Triggering;
                
                string oldGuid = originalEvent.RoutingGuid;
                originalEvent.RoutingGuid = newGuid;

                // Send it
                palWriter.PostMessage(originalEvent);
                return;
            }

            // Return failure response for unhandled triggering events.
            string remoteHost = originalEvent[Package.Events.GotRequest.Params.remoteHost.NAME] as string;
#if INTERNAL_STACK
            if(remoteHost == null)
            {
                // If using internal stack, extract the remoteHost string to make sure the session is still there.
                log.Write(TraceLevel.Error, "No handler without remote host, GUID=" + noHandlerAction.RoutingGuid);
                DebugLog.MethodExit();
                return;            
            }
#endif
            string url = originalEvent[Package.Events.GotRequest.Params.url.NAME] as string;
            log.Write(TraceLevel.Warning, "Request for resource '{0}' from {1} was not handled", url, remoteHost);

			HttpMessage notFoundMsg = new HttpMessage();
			notFoundMsg.uniqueId = noHandlerAction.RoutingGuid;
			notFoundMsg.responseCode = "404";
			notFoundMsg.type = HttpMessage.Type.Response;

			SendResponse(notFoundMsg, remoteHost);
		}
        #endregion

        #region Actions/Events
		/// <summary>Sends a session expiration event to the script
		/// instance that was triggered as a result of an incoming
		/// HTTP request.</summary>
		/// <param name="sessionKey">The routing GUID of the script instance.</param>
        [Event(Package.Events.SessionExpired.FULLNAME, eventTypeType.nontriggering, null, Package.Events.SessionExpired.DISPLAY, Package.Events.SessionExpired.DESCRIPTION)]
		public void SendExpiredSessionEvent(string sessionKey)
		{
			if(sessionKey == null) return;

			EventMessage sessionExpiredMsg =
                CreateEventMessage(Package.Events.SessionExpired.FULLNAME, EventMessage.EventType.NonTriggering, sessionKey);

			sessionExpiredMsg.SuppressNoHandler = true;

			palWriter.PostMessage(sessionExpiredMsg);
		}

        /// <summary>
        /// Request to end an active session
        /// </summary>
        /// <param name="actionBase"></param>
        [Action(Package.Actions.SessionEnd.FULLNAME, false, Package.Actions.SessionEnd.DISPLAY, Package.Actions.SessionEnd.DESCRIPTION, false)]
        public void HandleSessionEnd(ActionBase actionBase)
        {
            DebugLog.MethodEnter();
            
            SyncAction action = null;
            if(actionBase is SyncAction)
            {
                action = (SyncAction)actionBase;
            }
            else
            {
                action.SendResponse(false);
                DebugLog.MethodExit();
                return;
            }

            RemoveHttpSession(action.RoutingGuid);
            action.SendResponse(true);
            DebugLog.MethodExit();
        }

        [Action(Package.Actions.SendResponse.FULLNAME, true, Package.Actions.SendResponse.DISPLAY, Package.Actions.SendResponse.DESCRIPTION, false)]
        [ActionParam(Package.Actions.SendResponse.Params.remoteHost.NAME, Package.Actions.SendResponse.Params.remoteHost.DISPLAY, typeof(string), useType.required, false, Package.Actions.SendResponse.Params.remoteHost.DESCRIPTION, Package.Actions.SendResponse.Params.remoteHost.DEFAULT)]
        [ActionParam(Package.Actions.SendResponse.Params.responseCode.NAME, Package.Actions.SendResponse.Params.responseCode.DISPLAY, typeof(int), useType.required, false, Package.Actions.SendResponse.Params.responseCode.DESCRIPTION, Package.Actions.SendResponse.Params.responseCode.DEFAULT)]
        [ActionParam(Package.Actions.SendResponse.Params.responsePhrase.NAME, Package.Actions.SendResponse.Params.responsePhrase.DISPLAY, typeof(string), useType.optional, false, Package.Actions.SendResponse.Params.responsePhrase.DESCRIPTION, Package.Actions.SendResponse.Params.responsePhrase.DEFAULT)]
        [ActionParam(Package.Actions.SendResponse.Params.body.NAME, Package.Actions.SendResponse.Params.body.DISPLAY, typeof(string), useType.optional, false, Package.Actions.SendResponse.Params.body.DESCRIPTION, Package.Actions.SendResponse.Params.body.DEFAULT)]
        [ActionParam(Package.Actions.SendResponse.Params.Content_Type.NAME, Package.Actions.SendResponse.Params.Content_Type.DISPLAY, typeof(string), useType.optional, false, Package.Actions.SendResponse.Params.Content_Type.DESCRIPTION, Package.Actions.SendResponse.Params.Content_Type.DEFAULT)]
		public void HandleSendResponse(ActionBase actionBase)
		{
			DebugLog.MethodEnter();
            
			SyncAction action = null;
			if(actionBase is SyncAction)
			{
				action = (SyncAction) actionBase;
			}
			else
			{
				action.SendResponse(false);
				DebugLog.MethodExit();
				return;
			}

			string remoteHost;
			int responseCode;
			string phrase;
			string body;
			string contentType;

			bool phrasePresent;
			bool bodyPresent;
			bool contentTypePresent;

			try
			{
                action.InnerMessage.GetString(Package.Actions.SendResponse.Params.remoteHost.NAME, true, out remoteHost);
                action.InnerMessage.GetInt32(Package.Actions.SendResponse.Params.responseCode.NAME, true, out responseCode);
                phrasePresent = action.InnerMessage.GetString(Package.Actions.SendResponse.Params.responsePhrase.NAME, false, out phrase);
                bodyPresent = action.InnerMessage.GetString(Package.Actions.SendResponse.Params.body.NAME, false, out body);
                contentTypePresent = action.InnerMessage.GetString(Package.Actions.SendResponse.Params.Content_Type.NAME, false, out contentType);
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, e.Message);
				action.SendResponse(false);
				DebugLog.MethodExit();
				return;
			}

			lock(sessions.SyncRoot)
			{
				CreateNewHttpSession(action.RoutingGuid);
			}

            action.InnerMessage.RemoveField(Package.Actions.SendResponse.Params.remoteHost.NAME);

			// Put the InternalMessage into an HttpMessage
			HttpMessage hMsg = null;
			try
			{
				hMsg = new HttpMessage(action.InnerMessage, null);
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Warning, "Unable to create HTTP response. Error: " + e.ToString());
				action.SendResponse(false);
				DebugLog.MethodExit();
				return;
			}

			// Send it to the stack
			SendResponse(hMsg, remoteHost);

			// Respond to app
			action.SendResponse(true);
			DebugLog.MethodExit();
		}
        #endregion

        #region Stack/Listener manager
#if INTERNAL_STACK
		private void RefreshStack()
		{
				if(this.stack != null) stack.Stop();
				this.stack = new HttpStack(port, log.LogLevel);
				this.stack.onIncomingRequest += new HttpStack.IncomingRequestDelegate(HandleIncomingRequestCallback);
        }
#endif

		private void StartStack()
		{
#if INTERNAL_STACK
				this.stack.Start();
#else
            if(this.listener != null)
                listener.Stop();
            this.listener = new HttpListener(modPort);
            this.listener.onIncomingRequest += new HttpListener.IncomingRequestDelegate(HandleIncomingRequestCallback);
    		this.listener.Start();
#endif
		}

		private void StopStack()
		{
#if INTERNAL_STACK
			if (this.stack != null)
				this.stack.Stop();
#else
			if (this.listener != null)
				this.listener.Stop();
#endif
		}

		private void SendResponse(HttpMessage msg, string remoteHost)
		{
#if INTERNAL_STACK
				stack.SendResponse(msg, remoteHost);
#else
				listener.SendResponse(msg, remoteHost);
#endif
		}
        #endregion
	}
}
