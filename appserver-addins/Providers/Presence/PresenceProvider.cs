using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Core.ConfigData;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.PackageGeneratorCore.PackageXml;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Utilities;
using Metreos.Utilities.Collections;
using Metreos.Configuration;

using Package = Metreos.Interfaces.PackageDefinitions.Presence;

namespace Metreos.Providers.Presence
{
	/// <summary>
	/// Summary description for PresenceProvider.
	/// </summary>
    [ProviderDecl(Package.Globals.PACKAGE_NAME)]
	[PackageDecl(Package.Globals.NAMESPACE, Package.Globals.PACKAGE_DESCRIPTION)]
	public sealed class Presence : ProviderBase
	{
		#region Constants

		private abstract class Consts
		{
			public abstract class Defaults
			{
				public const int InitTimerThreads		= 1;	// There's no need for concurrent timer callbacks
				public const int MaxTimerThreads		= 1;    //   since we do very little work on timer fire
			}

            public abstract class ConfigValueNames
            {
                public const string ServiceLogLevel = "ServiceLogLevel";						//stack log levle
                public const string LogTimingStat   = "LogTimingStat";							//whether to log timing stats or not
                public const string ServiceTimeout  = "ServiceTimeout";
                public const string SubscriptionExpiration = "SubscribeExpires";
                public const string LogMessageBodies = "LogMessageBodies";
            }

            public abstract class DefaultValues
            {
                public const int ServiceLogLevel        = 2;    // Warning
                public const int ServiceTimeout         = 5;
                public const bool LogTimingStat		    = false;
                public const int SubscriptionExpiration = 300;
                public const bool LogMessageBodies      = false;
            }

            public abstract class Extensions
            {
                public const string PrintSubscriptions = "Metreos.Providers.Presence.PrintSubscriptions";
                public const string ClearSubscriptions = "Metreos.Providers.Presence.ClearSubscriptions";
                
                public abstract class Descriptions
                {
                    public const string PrintSubscriptions = "Print table of active subscriptions";
                    public const string ClearSubscriptions = "Unsubscribe all active subscriptions";
                }
            }
            
            public readonly static long MinuteMs		= Convert.ToInt64(TimeSpan.FromMinutes(1).TotalMilliseconds);
			public readonly static long HourMs			= Convert.ToInt64(TimeSpan.FromHours(1).TotalMilliseconds);
			public readonly static long DayMs			= Convert.ToInt64(TimeSpan.FromDays(1).TotalMilliseconds);

            public readonly static int Port             = 9510;

		}
		#endregion

        #region Subscription class

        public class Subscription
		{
            public Subscription(string subscriber, string password, 
                string requestUri, string routingGuid)
			{
				this.subscriber = subscriber;
				this.password = password;
				this.requestUri = requestUri;
                
				this.callId = null;
                this.active = false;
                this.triggering = false;
                this.unsubscribe = false;
                this.resultCode = (long) StackProxy.ResultCodes.Failure;

                this.routingGuids = new List<string>();
                if(routingGuid != null)
                    this.routingGuids.Add(routingGuid);
                else
                    this.triggering = true;
			}

			private string subscriber;
			public string Subscriber
			{
				get { return subscriber; }
				set { subscriber = value; }
			}

			private string requestUri;
			public string RequestUri
			{
				get { return requestUri; }
				set { requestUri = value; }
			}

			private string password;
			public string Password
			{
				get { return password; }
				set { password = value; }
			}

			private string callId;
			public string CallId
			{
				get { return callId; }
				set { callId = value; }
			}

			private bool active;
			public bool Active
			{
				get { return active; }
				set { active = value; }
			}

            private readonly List<string> routingGuids;
            public List<string> RoutingGuids
            {
                get { return routingGuids; }
            }

            private bool triggering;
            public bool Triggering
            {
                get { return triggering; }
                set { triggering = value; }
            }

            private bool unsubscribe;
            public bool Unsubscribe
            {
                get { return unsubscribe; }
                set { unsubscribe = value; }
            }

            private long resultCode;
            public long ResultCode
            {
                get { return resultCode; }
                set { resultCode = value; }
            }

            private string resultMsg;
            public string ResultMsg
            {
                get { return resultMsg; }
                set { resultMsg = value; }
            }
        }
        #endregion

        #region Member variables
        //There are 3 operations: Register, Subscribe, and Publish

		//the two key hash will hold all the current Subscription: <subscriber, requesturi> ==> <Subscription>
		TwoKeyHash subscriptions = new TwoKeyHash();
        Hashtable subLookup = new Hashtable();          //lookup table for subscriptions by callId

        StackProxy stackProxy = null;

        int serviceTimeout = Consts.DefaultValues.ServiceTimeout;


		#endregion

		public Presence(IConfigUtility configUtility)
			: base(typeof(Presence), Package.Globals.DISPLAY_NAME, configUtility)
		{
            this.stackProxy = CreateStackProxy(log);
		}

		#region ProviderBase Implementation

		/// <summary>Initialize the provider.</summary>
		/// <returns>True on success, false otherwise.</returns>
		protected override bool Initialize(out ConfigEntry[] configItems, out Extension[] extensions)
		{
            this.messageCallbacks.Add(Package.Actions.TriggeringSubscribe.FULLNAME, new HandleMessageDelegate(this.OnTriggeringSubscribe));
            this.messageCallbacks.Add(Package.Actions.NonTriggeringSubscribe.FULLNAME, new HandleMessageDelegate(this.OnNonTriggeringSubscribe));
            this.messageCallbacks.Add(Package.Actions.Unsubscribe.FULLNAME, new HandleMessageDelegate(this.OnUnsubscribe));

            this.messageCallbacks.Add(Consts.Extensions.PrintSubscriptions, new HandleMessageDelegate(this.PrintSubscriptions));
            this.messageCallbacks.Add(Consts.Extensions.ClearSubscriptions, new HandleMessageDelegate(this.ClearSubscriptions));
            
            // Set default config values
			configItems = new ConfigEntry[5];
            configItems[0] = new ConfigEntry(Consts.ConfigValueNames.ServiceLogLevel,
                Consts.ConfigValueNames.ServiceLogLevel,
                Consts.DefaultValues.ServiceLogLevel,
                "Presence service log level. 0=Off, 1=Error, 2=Warning, 3=Info, 4=Verbose",
                0, //min log level
                4, //max log level
                true);

            configItems[1] = new ConfigEntry(Consts.ConfigValueNames.ServiceTimeout,
                Consts.ConfigValueNames.ServiceTimeout,
                Consts.DefaultValues.ServiceTimeout,
                "The time(in seconds) provider waits for Presence service to respond. It should be a positive number.",
                1,
                int.MaxValue,
                true);

            configItems[2] = new ConfigEntry(Consts.ConfigValueNames.SubscriptionExpiration,
                Consts.ConfigValueNames.SubscriptionExpiration,
                Consts.DefaultValues.SubscriptionExpiration,
                "The expiration time(in seconds) for each subscription. When it expires, Presence service will automatically " +
                "re-subscribe to presence server for notification. The value must fall between " +
                "the configured minimum and maximum expires time on CUPS engine.",
                1,
                int.MaxValue,
                true);

            configItems[3] = new ConfigEntry(Consts.ConfigValueNames.LogTimingStat,
                Consts.ConfigValueNames.LogTimingStat,
                Consts.DefaultValues.LogTimingStat,
                "Set it to true to enable timing statistics",
                IConfig.StandardFormat.Bool,
                true);

            configItems[4] = new ConfigEntry(Consts.ConfigValueNames.LogMessageBodies,
                Consts.ConfigValueNames.LogMessageBodies,
                Consts.DefaultValues.LogMessageBodies,
                "Enables logging of Notify XML bodies",
                IConfig.StandardFormat.Bool,
                true);


            // No extensions
			extensions = new Extension[2];
            extensions[0] = new Extension(Consts.Extensions.PrintSubscriptions, Consts.Extensions.Descriptions.PrintSubscriptions);
            extensions[1] = new Extension(Consts.Extensions.ClearSubscriptions, Consts.Extensions.Descriptions.ClearSubscriptions);

			return true;
		}

		protected override void RefreshConfiguration()
		{
            int i = Convert.ToInt32(GetConfigValue(Consts.ConfigValueNames.ServiceLogLevel));
            bool paramChanged = (i != stackProxy.ServiceLogLevel);
            stackProxy.ServiceLogLevel = i;

            bool b = Convert.ToBoolean(GetConfigValue(Consts.ConfigValueNames.LogTimingStat));
            paramChanged = paramChanged || (b != stackProxy.LogTimingStat);
            stackProxy.LogTimingStat = b;

            b = Convert.ToBoolean(GetConfigValue(Consts.ConfigValueNames.LogMessageBodies));
            stackProxy.LogMessageBodies = b;

            serviceTimeout = Convert.ToInt32(GetConfigValue(Consts.ConfigValueNames.ServiceTimeout));

            i = Convert.ToInt32(GetConfigValue(Consts.ConfigValueNames.SubscriptionExpiration));
            paramChanged = paramChanged || (i != stackProxy.SubscribeExpires);
            stackProxy.SubscribeExpires = i;

            //config data has been read in and ready to be consumed
            stackProxy.ConfigDataReady.Set();

            if(paramChanged && stackProxy.Connected)
            {
                stackProxy.SendParameterChanged();
            }
        }


		protected override void OnStartup()
		{
            stackProxy.Startup(new IPEndPoint(IPAddress.Loopback, Consts.Port));
            this.RegisterNamespace();
		}

		protected override void OnShutdown()
		{
            ResetSubscriptionTable();

            stackProxy.Shutdown();
		}

		public override void Cleanup()
		{
			base.Cleanup();
		}

		#endregion

        #region Extensions

        private void PrintSubscriptions(ActionBase action)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Presence Provider Diags:");
            sb.AppendLine("     Subscriber      Trig  NTS         URI");
            sb.AppendLine("-------------------- ----- --- -------------------");
            foreach(Subscription sub in subscriptions.Values)
            {
                string subscriber = "<unknown>";
                if(sub.Subscriber != null && sub.Subscriber != String.Empty)
                    subscriber = sub.Subscriber;

                string requestUri = "<unknown>";
                if(sub.RequestUri != null && sub.RequestUri != String.Empty)
                    requestUri = sub.RequestUri;

                sb.Append(subscriber.PadRight(21));
                sb.Append(sub.Triggering.ToString().PadRight(6));
                sb.Append(sub.RoutingGuids.Count.ToString().PadRight(4));
                sb.AppendLine(requestUri);
            }

            log.ForceWrite(TraceLevel.Info, sb.ToString());
        }

        private void ClearSubscriptions(ActionBase action)
        {
            ResetSubscriptionTable();
        }

        #endregion

        #region Actions / Events

        /// <summary>Subscribe to presence notification.</summary>
		/// <param name="im">The message containing the add command.</param>
        [Action(Package.Actions.TriggeringSubscribe.FULLNAME, false, Package.Actions.TriggeringSubscribe.DISPLAY, Package.Actions.TriggeringSubscribe.DESCRIPTION, false)]
        [ActionParam(Package.Actions.TriggeringSubscribe.Params.requestUri.NAME, Package.Actions.TriggeringSubscribe.Params.requestUri.DISPLAY, typeof(string), useType.required, false, Package.Actions.TriggeringSubscribe.Params.requestUri.DESCRIPTION, Package.Actions.TriggeringSubscribe.Params.requestUri.DEFAULT)]
        [ActionParam(Package.Actions.TriggeringSubscribe.Params.subscriber.NAME, Package.Actions.TriggeringSubscribe.Params.subscriber.DISPLAY, typeof(string), useType.required, false, Package.Actions.TriggeringSubscribe.Params.subscriber.DESCRIPTION, Package.Actions.TriggeringSubscribe.Params.subscriber.DEFAULT)]
        [ActionParam(Package.Actions.TriggeringSubscribe.Params.password.NAME, Package.Actions.TriggeringSubscribe.Params.password.DISPLAY, typeof(string), useType.required, false, Package.Actions.TriggeringSubscribe.Params.password.DESCRIPTION, Package.Actions.TriggeringSubscribe.Params.password.DEFAULT)]
        [ResultData(Package.Actions.TriggeringSubscribe.Results.resultCode.NAME, Package.Actions.TriggeringSubscribe.Results.resultCode.DISPLAY, typeof(long), Package.Actions.TriggeringSubscribe.Results.resultCode.DESCRIPTION)]
        private void OnTriggeringSubscribe(ActionBase action)
        {
            string subscriber;
            string requestUri;
            string passwd;

            try
            {
                action.InnerMessage.GetString(Package.Actions.TriggeringSubscribe.Params.subscriber.NAME, true, out subscriber);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                Field result = new Field(Package.Actions.TriggeringSubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.MissingParamSubscriber);
                action.SendResponse(false, result);
                return;
            }

            try
            {
                action.InnerMessage.GetString(Package.Actions.TriggeringSubscribe.Params.password.NAME, true, out passwd);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                Field result = new Field(Package.Actions.TriggeringSubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.MissingParamPassword);
                action.SendResponse(false, result);
                return;
            }

            try
            {
                action.InnerMessage.GetString(Package.Actions.TriggeringSubscribe.Params.requestUri.NAME, true, out requestUri);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                Field result = new Field(Package.Actions.TriggeringSubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.MissingParamRequestUri);
                action.SendResponse(false, result);
                return;
            }

            long resultCode = Subscribe(null, subscriber, requestUri, passwd);

            Field resultField = new Field(Package.Actions.TriggeringSubscribe.Results.resultCode.NAME, resultCode);
            action.SendResponse(resultCode == (long) StackProxy.ResultCodes.Success, resultField);
        }

        /// <summary>Subscribe to presence notification.</summary>
        /// <param name="im">The message containing the add command.</param>
        [Action(Package.Actions.NonTriggeringSubscribe.FULLNAME, false, Package.Actions.NonTriggeringSubscribe.DISPLAY, Package.Actions.NonTriggeringSubscribe.DESCRIPTION, false)]
        [ActionParam(Package.Actions.NonTriggeringSubscribe.Params.requestUri.NAME, Package.Actions.NonTriggeringSubscribe.Params.requestUri.DISPLAY, typeof(string), useType.required, false, Package.Actions.NonTriggeringSubscribe.Params.requestUri.DESCRIPTION, Package.Actions.NonTriggeringSubscribe.Params.requestUri.DEFAULT)]
        [ActionParam(Package.Actions.NonTriggeringSubscribe.Params.subscriber.NAME, Package.Actions.NonTriggeringSubscribe.Params.subscriber.DISPLAY, typeof(string), useType.required, false, Package.Actions.NonTriggeringSubscribe.Params.subscriber.DESCRIPTION, Package.Actions.NonTriggeringSubscribe.Params.subscriber.DEFAULT)]
        [ActionParam(Package.Actions.NonTriggeringSubscribe.Params.password.NAME, Package.Actions.NonTriggeringSubscribe.Params.password.DISPLAY, typeof(string), useType.required, false, Package.Actions.NonTriggeringSubscribe.Params.password.DESCRIPTION, Package.Actions.NonTriggeringSubscribe.Params.password.DEFAULT)]
        [ResultData(Package.Actions.NonTriggeringSubscribe.Results.resultCode.NAME, Package.Actions.NonTriggeringSubscribe.Results.resultCode.DISPLAY, typeof(long), Package.Actions.NonTriggeringSubscribe.Results.resultCode.DESCRIPTION)]
        private void OnNonTriggeringSubscribe(ActionBase action)
        {
            string subscriber;
            string requestUri;
            string passwd;

            try
            {
                action.InnerMessage.GetString(Package.Actions.NonTriggeringSubscribe.Params.subscriber.NAME, true, out subscriber);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                Field result = new Field(Package.Actions.NonTriggeringSubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.MissingParamSubscriber);
                action.SendResponse(false, result);
                return;
            }

            try
            {
                action.InnerMessage.GetString(Package.Actions.NonTriggeringSubscribe.Params.password.NAME, true, out passwd);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                Field result = new Field(Package.Actions.NonTriggeringSubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.MissingParamPassword);
                action.SendResponse(false, result);
                return;
            }

            try
            {
                action.InnerMessage.GetString(Package.Actions.NonTriggeringSubscribe.Params.requestUri.NAME, true, out requestUri);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                Field result = new Field(Package.Actions.NonTriggeringSubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.MissingParamRequestUri);
                action.SendResponse(false, result);
                return;
            }

            long resultCode = Subscribe(action.InnerMessage.RoutingGuid, subscriber, requestUri, passwd);

            Field resultField = new Field(Package.Actions.NonTriggeringSubscribe.Results.resultCode.NAME, resultCode);
            action.SendResponse(resultCode == (long) StackProxy.ResultCodes.Success, resultField);
        }


        private long Subscribe(string routingGuid, string subscriber, string requestUri, string passwd)
        {
            SipDomainInfo di = GetDomainInfo(subscriber);
            if(di == null)
            {
                log.Write(TraceLevel.Error, "OnSubscribe: Unknown domain name associcated with requestUri: " + requestUri);
                return (long) StackProxy.ResultCodes.UnknownDomainName;
            }

            Subscription sub = LookupSubscription(subscriber, requestUri);
            if(sub == null)
            {
                sub = new Subscription(subscriber, passwd, requestUri, routingGuid);
                AddSubscription(subscriber, requestUri, sub);

                log.Write(TraceLevel.Info, "Added " + (sub.Triggering ? "" : "non-") + "triggering presence subscription request for " + subscriber + " to " + requestUri);
            }
            else if(routingGuid != null)
            {
                lock(sub.RoutingGuids)
                {
                    if(!sub.RoutingGuids.Contains(routingGuid))
                        sub.RoutingGuids.Add(routingGuid);
                }
            }
            else
            {
                sub.Triggering = true;
            }

            //forward the request to sip stack
            SendSubscribe(sub);
            return sub.ResultCode;
        }

        /// <summary>
		/// Remove a timer callback.
		/// </summary>
		/// <param name="im">The message containing the unsubscribe command.</param>
		[Action(Package.Actions.Unsubscribe.FULLNAME, false, Package.Actions.Unsubscribe.DISPLAY, Package.Actions.Unsubscribe.DESCRIPTION, false)]
        [ActionParam(Package.Actions.Unsubscribe.Params.subscriber.NAME, Package.Actions.Unsubscribe.Params.subscriber.DISPLAY, typeof(string), useType.required, false, Package.Actions.Unsubscribe.Params.subscriber.DESCRIPTION, Package.Actions.Unsubscribe.Params.subscriber.DEFAULT)]
        [ActionParam(Package.Actions.Unsubscribe.Params.password.NAME, Package.Actions.Unsubscribe.Params.password.DISPLAY, typeof(string), useType.optional, false, Package.Actions.Unsubscribe.Params.password.DESCRIPTION, Package.Actions.Unsubscribe.Params.password.DEFAULT)]
        [ActionParam(Package.Actions.Unsubscribe.Params.requestUri.NAME, Package.Actions.Unsubscribe.Params.requestUri.DISPLAY, typeof(string), useType.required, false, Package.Actions.Unsubscribe.Params.requestUri.DESCRIPTION, Package.Actions.Unsubscribe.Params.requestUri.DEFAULT)]
        [ActionParam(Package.Actions.Unsubscribe.Params.triggering.NAME, Package.Actions.Unsubscribe.Params.triggering.DISPLAY, typeof(bool), useType.required, false, Package.Actions.Unsubscribe.Params.triggering.DESCRIPTION, Package.Actions.Unsubscribe.Params.triggering.DEFAULT)]
        [ResultData(Package.Actions.Unsubscribe.Results.resultCode.NAME, Package.Actions.Unsubscribe.Results.resultCode.DISPLAY, typeof(long), Package.Actions.NonTriggeringSubscribe.Results.resultCode.DESCRIPTION)]
        private void OnUnsubscribe(ActionBase action)
		{
			string subscriber;
            string password;
			string requestUri;
            bool triggering;
            Field result;

			try
			{
                action.InnerMessage.GetString(Package.Actions.Unsubscribe.Params.subscriber.NAME, true, out subscriber);
			}
			catch(Exception e)
			{
				log.Write(TraceLevel.Error, e.Message);
                result = new Field(Package.Actions.Unsubscribe.Results.resultCode.NAME, 
                                    StackProxy.ResultCodes.MissingParamSubscriber);
				action.SendResponse(false, result);
				return;
			}

            try
            {
                action.InnerMessage.GetString(Package.Actions.Unsubscribe.Params.password.NAME, true, out password);
            }
   			catch(Exception e)
			{
				log.Write(TraceLevel.Error, e.Message);
                result = new Field(Package.Actions.Unsubscribe.Results.resultCode.NAME, 
                                    StackProxy.ResultCodes.MissingParamPassword);
				action.SendResponse(false, result);
				return;
			}

            try
            {
                action.InnerMessage.GetString(Package.Actions.Unsubscribe.Params.requestUri.NAME, true, out requestUri);
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, e.Message);
                result = new Field(Package.Actions.Unsubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.MissingParamRequestUri);
                action.SendResponse(false, result);
                return;
            }

            try
            {
                action.InnerMessage.GetBoolean(Package.Actions.Unsubscribe.Params.triggering.NAME, true, out triggering);
            }
            catch
            {
                log.Write(TraceLevel.Warning, "Invalid triggering indication found in Unsubscribe. Assuming non-triggering...");
                triggering = false;
            }

            log.Write(TraceLevel.Verbose, "Unsubscribe ({0}triggering) presence for: {1} -> {2}", triggering ? "" : "non-", subscriber, requestUri);

            //should I check and see if there is an outstanding subscription first?
            Subscription sub = LookupSubscription(subscriber, requestUri);
            if(sub == null)
            {
                log.Write(TraceLevel.Warning, "There is no outstanding subscription to unsubscribe from for: " + subscriber + " -> " + requestUri);
                result = new Field(Package.Actions.Unsubscribe.Results.resultCode.NAME,
                                    StackProxy.ResultCodes.NoSubscription);
                action.SendResponse(true, result);
                return;
            }

            Unsubscribe(sub, triggering ? null : action.InnerMessage.RoutingGuid);

            result = new Field(Package.Actions.Unsubscribe.Results.resultCode.NAME,
                                sub.ResultCode);

            action.SendResponse(sub.ResultCode==(long)StackProxy.ResultCodes.Success, result);
		}

        private void Unsubscribe(Subscription sub, string routingGuid)
        {
            if(sub == null)
                return;

            bool unsubscribe = false;

            if(routingGuid != null)
            {
                lock(sub.RoutingGuids)
                {
                    if(sub.RoutingGuids.Contains(routingGuid))
                        sub.RoutingGuids.Remove(routingGuid);

                    if(!sub.Triggering && sub.RoutingGuids.Count == 0)
                        unsubscribe = true;
                }

            }
            else
            {
                sub.Triggering = false;

                if(sub.RoutingGuids.Count == 0)
                    unsubscribe = true;
            }

            if(unsubscribe)
            {
                sub.Active = false;
                sub.Unsubscribe = true;
                SendSubscribe(sub);
            }
        }

        [Event(Package.Events.SubscriptionTerminated.FULLNAME, eventTypeType.hybrid, null, Package.Events.SubscriptionTerminated.DISPLAY, Package.Events.SubscriptionTerminated.DESCRIPTION)]
        [EventParam(Package.Events.SubscriptionTerminated.Params.subscriber.NAME, Package.Events.SubscriptionTerminated.Params.subscriber.DISPLAY, typeof(string), true, Package.Events.SubscriptionTerminated.Params.subscriber.DESCRIPTION)]
        [EventParam(Package.Events.SubscriptionTerminated.Params.requestUri.NAME, Package.Events.SubscriptionTerminated.Params.requestUri.DISPLAY, typeof(string), true, Package.Events.SubscriptionTerminated.Params.requestUri.DESCRIPTION)]
        private void SubscriptionTerminated() { }

        [Event(Package.Events.Notify.FULLNAME, eventTypeType.hybrid, null, Package.Events.Notify.DISPLAY, Package.Events.Notify.DESCRIPTION)]
        [EventParam(Package.Events.Notify.Params.subscriber.NAME, Package.Events.Notify.Params.subscriber.DISPLAY, typeof(string), true, Package.Events.Notify.Params.subscriber.DESCRIPTION)]
        [EventParam(Package.Events.Notify.Params.requestUri.NAME, Package.Events.Notify.Params.requestUri.DISPLAY, typeof(string), true, Package.Events.Notify.Params.requestUri.DESCRIPTION)]
        [EventParam(Package.Events.Notify.Params.status.NAME, Package.Events.Notify.Params.status.DISPLAY, typeof(string), true, Package.Events.Notify.Params.status.DESCRIPTION)]
        private void Notify() { }

		#endregion 

		#region Callbacks
        
        /// <summary>
        /// Callback for StackStarted event. It will re-subscribe to presence service for presence 
        /// notification in current subscription list.
        /// </summary>
        private void OnStackStarted()
        {
            //if there are inactive subscriptions in the list, auto re-subscribe
            bool rc;
            log.Write(TraceLevel.Info, "Presence service started, re-subscribe for notifications.");
            foreach(Subscription sub in subscriptions.Values)
            {
                log.Write(TraceLevel.Verbose, "Re-subscribe for: subscriber={0}, requestUri={1}", 
                    sub.Subscriber, sub.RequestUri);
                
                rc = SendSubscribe(sub);

                log.Write(TraceLevel.Verbose, "Re-subscribe for: subscriber={0}, requestUri={1}, result={2}",
                    sub.Subscriber, sub.RequestUri, rc);
            }
        }

        private void OnServiceGone()
        {
            foreach(Subscription sub in subscriptions.Values)
            {
                //PostEventMessage(Consts.Events.SubscriptionTerminated, sub.Subscriber, sub.RequestUri);
                sub.Active = false;
                sub.CallId = null;
            }
        }

        private void OnError(string subscriber, string requestUri, string msg, long result)
        {
            log.Write(TraceLevel.Error, "Received an error for subscriber:{0}, requestUri:{1}: {2} resultCode: {3}", 
                subscriber, requestUri, msg, result);
        }

        private void OnRegisterAck(string requestUri, string stackCallId, long result)
        {
            //what to do with this ack?
        }

        private void OnPublishAck(string requestUri, string stackCallId, long result)
        {
            //what to do with this ack?
        }

        private void OnSubscribeAck(string subscriber, string requestUri, 
                                    string stackCallId, long result, string resultMsg)
        {
            log.Write(TraceLevel.Verbose, "Start of OnSubscribeAck: subscriber={0}, requestUri={1}, result={2}",
                subscriber, requestUri, result);

            //look up the subscription in the table
            Subscription sub = LookupSubscription(subscriber, requestUri);
            if (sub == null)
            {
                //unknown subscription, log an error
                log.Write(TraceLevel.Warning, "Received SubScribeAck for an unknown subscription: subscriber={0} requestUri={1}",
                    subscriber, requestUri);
            }
            else
            {
                lock(sub)
                {
                    sub.CallId = stackCallId;
                    sub.Active = ((StackProxy.ResultCodes) result) == StackProxy.ResultCodes.Success;
                    sub.ResultCode = result;

                    if(sub.Active)
                        sub.ResultMsg = "";
                    else
                        sub.ResultMsg = resultMsg;

                    Monitor.Pulse(sub);
                }
            }

            log.Write(TraceLevel.Verbose, "End of OnSubscribeAck: subscriber={0}, requestUri={1}, result={2}",
                subscriber, requestUri, result);
        }

        private void OnNotify(string subscriber, string requestUri, string stackCallId, string status)
        {
            //look up the subscription in the table
            Subscription sub = LookupSubscription(subscriber, requestUri);
            if (sub == null)
            {
                //unknown subscription, log an error
                log.Write(TraceLevel.Warning, "Received OnNotify for an unknown subscription: subscriber={0}, requestUri={1}",
                    subscriber, requestUri);

                //unsubscribe it
                sub = new Subscription(subscriber, "", requestUri, null);
                sub.Unsubscribe = true;
                SendSubscribe(sub);
            }
            else
            {
                //this Notify may serve as acknowledgment for Subscribe request.
                //if that's the case, subscribe will be waiting for this signal
                //to proceed.
                lock(sub)
                {
                    //positive subscription result, set the result code
                    sub.ResultCode = (long) StackProxy.ResultCodes.Success;
                    sub.ResultMsg = "";

                    if(!sub.Unsubscribe) //if unsubscribing, throw away the Notify
                    {
                        sub.CallId = stackCallId;

                        EventMessage im;
                        if(sub.Triggering)
                        {
                            im = this.CreateEventMessage(
                                Package.Events.Notify.FULLNAME,
                                EventMessage.EventType.Triggering,
                                System.Guid.NewGuid().ToString());

                            im.AddField(Package.Events.Notify.Params.subscriber.NAME, sub.Subscriber);
                            im.AddField(Package.Events.Notify.Params.requestUri.NAME, sub.RequestUri);
                            im.AddField(Package.Events.Notify.Params.status.NAME, status);

                            palWriter.PostMessage(im);

                            log.Write(TraceLevel.Verbose, "A triggering Notify (subscriber={0}, requestUri={1}) has fired.", 
                                sub.Subscriber, sub.RequestUri);
                        }

                        lock(sub.RoutingGuids)
                        {
                            foreach(string routingGuid in sub.RoutingGuids)
                            {
                                im = this.CreateEventMessage(
                                    Package.Events.Notify.FULLNAME,
                                    EventMessage.EventType.NonTriggering,
                                    routingGuid);

                                im.AddField(Package.Events.Notify.Params.subscriber.NAME, sub.Subscriber);
                                im.AddField(Package.Events.Notify.Params.requestUri.NAME, sub.RequestUri);
                                im.AddField(Package.Events.Notify.Params.status.NAME, status);

                                palWriter.PostMessage(im);

                                log.Write(TraceLevel.Verbose, "A non-triggering Notify (subscriber={0}, requestUri={1} has fired.", sub.Subscriber, sub.RequestUri);
                            }
                        }
                    }//if(sub.unsubscribe)
                    else
                    {
                        log.Write(TraceLevel.Verbose, "Got a Notify message for unsubscribing, ignore it.");
                    }

                    Monitor.Pulse(sub);
                }
            }//lock
        }

        private void OnSubscriptionTerminated(string subscriber, string requestUri, string reason, long resultCode)
        {
            //look up the subscription in the table
            Subscription sub = LookupSubscription(subscriber, requestUri);

            if(sub == null)
            {
                //unknown subscription, log an error
                log.Write(TraceLevel.Warning, "Received OnSubscriptionTerminated for an unknown subscription: subscriber={0}, requestUri={1}",
                    subscriber, requestUri);
            }
            else
            {
                //this SubscriptionTerminated may serve as acknowledgment for Subscribe request.
                //if that's the case, subscribe will be waiting for this signal
                //to proceed.
                lock(sub)
                {
                    sub.ResultCode = resultCode;
                    sub.ResultMsg = reason;
                    Monitor.Pulse(sub);
                }

                if(sub.Active)
                {
                    EventMessage im;
                    if(sub.Triggering)
                    {
                        im = this.CreateEventMessage(
                            Package.Events.SubscriptionTerminated.FULLNAME,
                            EventMessage.EventType.Triggering,
                            System.Guid.NewGuid().ToString());

                        im.AddField(Package.Actions.TriggeringSubscribe.Params.subscriber.NAME, sub.Subscriber);
                        im.AddField(Package.Actions.TriggeringSubscribe.Params.requestUri.NAME, sub.RequestUri);

                        palWriter.PostMessage(im);

                        log.Write(TraceLevel.Verbose, "A triggering {0} has been posted for (subscriber={1}, requestUri={2}).",
                            Package.Events.SubscriptionTerminated.FULLNAME, sub.Subscriber, sub.RequestUri);
                    }

                    lock(sub.RoutingGuids)
                    {
                        foreach(string routingGuid in sub.RoutingGuids)
                        {
                            im = this.CreateEventMessage(
                                Package.Events.SubscriptionTerminated.FULLNAME,
                                EventMessage.EventType.NonTriggering,
                                routingGuid);

                            log.Write(TraceLevel.Verbose, "A non-triggering {0} has been posted for (subscriber={1}, requestUri={2}.",
                                Package.Events.SubscriptionTerminated.FULLNAME, sub.Subscriber, sub.RequestUri);

                            im.AddField(Package.Actions.TriggeringSubscribe.Params.subscriber.NAME, sub.Subscriber);
                            im.AddField(Package.Actions.TriggeringSubscribe.Params.requestUri.NAME, sub.RequestUri);

                            palWriter.PostMessage(im);
                        }
                    }
                }

                RemoveSubscription(subscriber, requestUri);
            }
        }

        protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
		{
            // We only care about NoHandlers for Notify
            if(originalEvent.MessageId != Package.Events.Notify.FULLNAME)
                return;

            string msg = null;
            string subscriber = originalEvent[Package.Actions.TriggeringSubscribe.Params.subscriber.NAME] as string;
            string requestUri = originalEvent[Package.Actions.TriggeringSubscribe.Params.requestUri.NAME] as string;

            log.Write(TraceLevel.Verbose, "There is no handler for event: {0}", originalEvent.ToString());
 
            Subscription sub;
			if(subscriber != null && requestUri != null)
			{
                sub = LookupSubscription(subscriber, requestUri);
				if(sub != null)
				{
                    Unsubscribe(sub, originalEvent.RoutingGuid);
				}
				else
				{
					msg = "unable to find the subscription in the subscription list for subscriber: " + subscriber 
                        + " requestUri: " + requestUri;
				}
			}
			else
			{
				msg = "unable to extract subscriber and requestUri from the no-handler message.";
			}

            if(msg != null)
            {
                log.Write(TraceLevel.Warning, "Received a no handler, but could not remove the subscription,"
				    + " because the Presence Provider was {0}", msg);
            }
		}

		#endregion 

		#region Utilities
        private StackProxy CreateStackProxy(LogWriter log)
        {
            StackProxy p = new StackProxy(log);

            p.onStackStarted = OnStackStarted;
            p.onServiceGone = OnServiceGone;
            p.onRegisterAck = OnRegisterAck;
            p.onSubscribeAck = OnSubscribeAck;
            p.onPublishAck = OnPublishAck;
            p.onNotify = OnNotify;
            p.onSubscriptionTerminated = OnSubscriptionTerminated;

            p.onError = OnError;

            return p;
        }


		/// <summary>
		/// Reset the timer table. This will dispose of all the timers currently in the
		/// timer table hash.
		/// </summary>
		public void ResetSubscriptionTable()
		{
            ArrayList subs = new ArrayList(subscriptions.Values);
			foreach(Subscription sub in subs)
			{
                if(!sub.Unsubscribe)
                {
                    sub.Active = !this.shutdownRequested;  // Don't send SubscriptionTerminated if shutting down
                    sub.Unsubscribe = true;
                    SendSubscribe(sub);
                }
			}

			subscriptions.Clear();
            subLookup.Clear();
		}

		private void SendSubscritionRequestComplete(ActionBase action, string subscriber, string requestUri)
		{
			log.Write(TraceLevel.Verbose, "Sending subscribe to presence request complete for: " + subscriber + " -> " + requestUri);
			ArrayList fields = new ArrayList();
            fields.Add(new Field(Package.Actions.TriggeringSubscribe.Params.subscriber.NAME, subscriber));
            fields.Add(new Field(Package.Actions.TriggeringSubscribe.Params.requestUri.NAME, requestUri));
            fields.Add(new Field(Package.Actions.TriggeringSubscribe.Params.appName.NAME, action.InnerMessage.AppName));
            action.SendResponse(true, fields);
		}

		private void SendSubscriptionRequestFailed(ActionBase action, string subscriber, string requestUri)
		{
			log.Write(TraceLevel.Verbose, "Sending subscribe to presence request failed for: " + subscriber + " -> " + requestUri);
        
            ArrayList fields = new ArrayList();
            fields.Add(new Field(Package.Actions.TriggeringSubscribe.Params.subscriber.NAME, subscriber));
            fields.Add(new Field(Package.Actions.TriggeringSubscribe.Params.requestUri.NAME, requestUri));
            fields.Add(new Field(Package.Actions.TriggeringSubscribe.Params.appName.NAME, action.InnerMessage.AppName));
            action.SendResponse(false, fields);
        }

        private SipDomainInfo GetDomainInfo(string uri)
        {
            //parse out the domain name from requestUri
            string domain = null;
            SipDomainInfo di = null;
            int pos = uri.IndexOf('@');
            if(pos >= 0)
                domain = uri.Substring(pos+1);
            if(domain != null && domain.Length > 0)
                di = configUtility.GetSipDomainInfo(domain);

            return di;
        }

        /// <summary>
        /// Send the subscribe/unsuscribe request to presence service and wait
        /// for a response back. It will block until an acknowledgement is received
        /// or the timeout has occurred.
        /// 
        /// </summary>
        /// <param name="sub">the subscription information</param>
        /// <param name="unsubscribe">indicates whether this is to unsubscribe</param>
        /// <returns>true if receives a positive response from presence service.</returns>
        private bool SendSubscribe(Subscription sub)
        {
            bool rc = false;

            //parse out the domain name from requestUri
            SipDomainInfo di = GetDomainInfo(sub.Subscriber);
            if(di == null)
            {
                log.Write(TraceLevel.Error, "Unknown domain name associcated with requestUri: " + sub.RequestUri);
                sub.ResultCode = (long) StackProxy.ResultCodes.UnknownDomainName;
                RemoveSubscription(sub.Subscriber, sub.RequestUri);
                return rc;
            }

            //forward the request to sip stack
            if(!stackProxy.SendSubscribe(sub, di))
            {
                log.Write(TraceLevel.Error, "Failed to send {0} request to Presence Service for: subscriber={1}, requestUri={2}",
                    sub.Unsubscribe ? "Unsubscribe" : "Subscribe", sub.Subscriber, sub.RequestUri);
                sub.ResultCode = (long) StackProxy.ResultCodes.ServiceNotAvailable;
                RemoveSubscription(sub.Subscriber, sub.RequestUri);
                return rc;
            }

            //block and wait for response
            log.Write(TraceLevel.Verbose, "Subscribe request is sent, waiting for SubscribeAck from stack...");
            lock(sub)
            {
                if(!Monitor.Wait(sub, serviceTimeout*1000))
                {
                    log.Write(TraceLevel.Error, "Timed out while waiting for SubscribeAck from stack.");
                    sub.ResultCode = (long) StackProxy.ResultCodes.Timeout;
                    return false;
                }

                log.Write(TraceLevel.Verbose, "Got SubscribeAck from stack with result code: " + sub.Active);
                return sub.Active;
            }
        }

        private void AddSubscription(string subscriber, string requestUri, Subscription sub)
        {
            subscriptions.Add(subscriber, requestUri, sub);
            if(sub.CallId != null && sub.CallId.Length > 0)
                subLookup.Add(sub.CallId, sub);
        }

        private Subscription RemoveSubscription(string subscriber, string requestUri)
        {
            Subscription sub = LookupSubscription(subscriber, requestUri);
            if(sub != null && sub.CallId != null && sub.CallId.Length > 0)
                subLookup.Remove(sub.CallId);
            subscriptions.Remove(subscriber, requestUri);

            return sub;
        }

        private Subscription LookupSubscription(string subscriber, string requestUri)
        {
            return subscriptions[subscriber, requestUri] as Subscription;
        }

        private Subscription LookupSubscription(string callId)
        {
            return subLookup[callId] as Subscription;
        }

        #endregion 
	}
}
