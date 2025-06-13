using System;
using System.Text;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.IO;

using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.Sockets;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.Core.IPC.Flatmaps;
using Metreos.Configuration;

using Utils=Metreos.Utilities;

namespace Metreos.Providers.Presence
{
    public delegate void VoidDelegate();
    public delegate void OnErrorDelegate(string subscriber, string requestUri, string message, long result); 
    public delegate void OnRegisterAckDelegate(string subscriber, string stackCallId, long result);
    public delegate void OnSubscribeAckDelegate(string subscriber, string requestUri, string stackCallId, long result, string resultMsg);
    public delegate void OnPublishAckDelegate(string requestUri, string stackCallId, long result);
    public delegate void OnNotifyDelegate(string subscriber, string requestUri, string stackCallId, string status);
    public delegate void OnSubscriptionTerminatedDelegate(string subscriber, string requestUri, string reason, long result);

	/// <summary>Flatmap IPC abstraction layer</summary>
	public class StackProxy : IDisposable
	{
		private delegate void ProcessMessageDelegate(int messageType, FlatmapList flatmap);

		#region Constants

		private abstract class Consts
		{
			public const int IpcWriteTimeout    = 5;    // seconds
			public const int IpcConnectTimeout  = 120;  // seconds
			public const int NumThreads         = 3;
			public const int MaxThreads         = 10;
			public const string PoolName        = "Presence ThreadPool";
			public const string ConnectorThreadName = "Presence service connect thread";
			public const int PresencePort			= 6050;
		}

		#endregion

		#region Message definitions

		public enum MsgType
		{
			Error				= 0,    // Causes message loop to exit
			Quit                = 1,    // Causes message loop to exit
			InternalInit        = 2,    // 1-15 should not be user-handled 
			Ping                = 16,   // Generic thread ping
			PingBack            = 17,   // Generic thread acknowledge
			Start               = 18,   // Start the runtime
			Stop                = 19,   // Stop the runtime

			StartStack			= 129,  // IPC: Start the stack
			StopStack			= 130,  // IPC: Stop the stack
			StartStackAck		= 131,
			StopStackAck		= 132,
      
			//messages from the stack
			RegisterAck		    = 140,
			SubscribeAck		= 141,
			PublishAck			= 142,
			Notify              = 143,
            SubscriptionTerminated = 144,

            ParameterChanged	= 201,
            Register            = 202,
            Unregister          = 203,
            Subscribe           = 204,
            Unsubscribe         = 205,
            Publish             = 206
		}

		public enum MsgField
		{
			ResultCode          = 0,
			ResultMsg			= 1,	
			StackCallId			= 2,
			Subscriber          = 3,
			RequestUri			= 4,
			Password            = 5,
            ServiceLogLevel     = 6,
			EnableDebug			= 7,
			DebugLevel			= 8,
			DebugFilename		= 9,
			ListenPort			= 10,
            SipPort             = 11,
			Registrars			= 12,
			ProxyServer			= 13,
			DomainName			= 14,
			Status				= 15,
			LogTimingStat		= 16,
            FailReason          = 17,
            Pidf                = 18,
            ServiceTimeout      = 19,
            SubscribeExpires    = 20,
            Reason              = 21,
            AppName             = 22
		}

		public enum FailReason 
		{
			UnknownMessageType  = 4,
			GeneralFailure      = 9,
			MissingField        = 13
		}

		public enum Status 
		{
			SubscriberUnregistered		= 0,
			SubscriberRegistered		= 1,
			SubscriberFailedToRegister	= 2
		}

		public enum ResultCodes : long
		{
			Success			        = 0,
			Failure			        = 1,
            DuplicateSubscription	= 2,
            MissingParamSubscriber	= 3,
            MissingParamRequestUri	= 4,
            MissingParamPassword	= 5,
            MissingParamAppName		= 6,
            BadSubscriberFormat		= 7,
            BadRequestUriFormat		= 8,
            MissingRegistrarInfo	= 9,
            MissingDomainName		= 10,
            UnknownDomainName       = 11,
            NoSubscription          = 12,
            ServiceNotAvailable     = 13,
            Timeout                 = 14,
            Unauthorized            = 15,
            AuthenticationFailed    = 16,
		}

		#endregion

		private LogWriter log;
		private IpcFlatmapClient ipcClient;
		private Utils.ThreadPool threadPool;

		private volatile bool connected = false;
		public bool Connected
		{
			get { lock(this) { return connected; } }
		}

		private volatile bool stackInitialized = false;

		private ManualResetEvent configDataReady = new ManualResetEvent(false);
		public ManualResetEvent ConfigDataReady
		{
			get { return configDataReady; }
		}

		//Log level for sip stack
		private int serviceLogLevel;
		public int ServiceLogLevel
		{
			get { return serviceLogLevel; }
			set { serviceLogLevel = value; }
		}

		private bool logTimingStat;
		public bool LogTimingStat
		{
			get { return logTimingStat; }
			set { logTimingStat = value; }
		}

        private bool logMessageBodies;
        public bool LogMessageBodies
        {
            get { return logMessageBodies; }
            set { logMessageBodies = value; }
        }

        private int subscribeExpires;
        public int SubscribeExpires
        {
            get { return subscribeExpires; }
            set { subscribeExpires = value; }
        }

        // Internal data delegates
        public VoidDelegate onServiceGone;

        // Incoming message-handler delegates
        public OnRegisterAckDelegate onRegisterAck;
        public OnSubscribeAckDelegate onSubscribeAck;
        public OnPublishAckDelegate onPublishAck;
        public OnNotifyDelegate onNotify;
        public OnErrorDelegate onError;
        public VoidDelegate onStackStarted;
        public OnSubscriptionTerminatedDelegate onSubscriptionTerminated;

        // ThreadPool callback delegates
        private ProcessMessageDelegate processMessageAsync;
        
        #region Construction/Startup/Shutdown/Cleanup

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="log">where the logs go</param>
		public StackProxy(LogWriter log)
		{
            Assertion.Check(log != null, "null log passed into SipProxy constructor");
            
            this.log = log;
			lock(this)
			{
				this.connected = false;
			}

            this.processMessageAsync = new ProcessMessageDelegate(ProcessMessageAsync);

            this.threadPool = new Utils.ThreadPool(Consts.NumThreads, Consts.MaxThreads, Consts.PoolName);

            this.ipcClient = new IpcFlatmapClient();
			this.ipcClient.onConnect = new OnConnectDelegate(ipcClient_onConnect);
            this.ipcClient.onFlatmapMessageReceived = new OnFlatmapMessageReceivedDelegate(ipcClient_OnMessageReceived);
			this.ipcClient.onClose = new OnCloseDelegate(ipcClient_onConnectionClosed);
		}

		/// <summary>
		/// Proxy start up
		/// </summary>
		/// <param name="ipEndpoint">the other end of pipe</param>
		/// <returns></returns>
        public bool Startup(IPEndPoint ipEndpoint)
        {
            if(ipEndpoint == null || ipEndpoint.Address == null || ipEndpoint.Port == 0)
                return false;

            if(this.threadPool == null)
                throw new ObjectDisposedException(typeof(StackProxy).Name);

            if(this.threadPool.IsStarted == false)              
                this.threadPool.Start();

            if(ipcClient == null)
                throw new ObjectDisposedException(typeof(StackProxy).Name);

            ipcClient.RemoteEp = ipEndpoint;
			ipcClient.Start();
			return true;
        }

		/// <summary>
		/// It shuts down the proxy to stack and stops the thread pool.
		/// </summary>
        public void Shutdown()
        {
            if(threadPool != null)
                threadPool.Stop();

            if(ipcClient != null)
                ipcClient.Close();
        }

		/// <summary>
		/// It frees the resources.
		/// </summary>
        public void Dispose()
        {
            if(ipcClient != null)
            {
                ipcClient.Dispose();
                ipcClient = null;
            }

            if(threadPool != null)
            {
                threadPool.Stop();
                threadPool = null;
            }
        }

		/// <summary>
		/// It sends startup request to stack.
		/// </summary>
        private void InitializeService()
        {
            if(ipcClient == null) 
                return;

			SendStartStack();
        }

        #endregion

        #region IPC Client events

		/// <summary>
		/// Callback function for successful connection to stack
		/// </summary>
		/// <param name="c">the ipc connection</param>
		/// <param name="reconnect">indicates whethere its a reconnect or not</param>
        private void ipcClient_onConnect(IpcClient c, bool reconnect)
        {
            log.Write(TraceLevel.Info, "Connected to Presence service successfully");
			whineAboutClose = true;
			lock(this)
			{
				connected = true;
			}

            InitializeService();
        }

		private bool whineAboutClose = true;

		/// <summary>
		/// Callback function for connection close to stack
		/// </summary>
		/// <param name="c">the connection</param>
		/// <param name="e">the exception generated by the close</param>
        private void ipcClient_onConnectionClosed(IpcClient c, Exception e)
        {
			if (whineAboutClose)
			{
				log.Write(TraceLevel.Warning, "Lost connection to Presence service: {0}",
					e != null ? e.Message : "(no msg)" );
				lock(this)
				{
					connected = false;
				}
				whineAboutClose = false;
			}

			if (onServiceGone != null)
	            onServiceGone();
        }

		/// <summary>
		/// Callback function for arrival of messages from stack
		/// </summary>
		/// <param name="client">the connection</param>
		/// <param name="messageType">type of the message arrived</param>
		/// <param name="flatmap">the message itself</param>
        private void ipcClient_OnMessageReceived(IpcFlatmapClient client, int messageType, FlatmapList flatmap)
        {
            log.Write(TraceLevel.Verbose, "Got {0}({1}) message.", messageType, ((MsgType)messageType).ToString());
            
            threadPool.PostRequest(processMessageAsync, new object[] { messageType, flatmap });
        }

		/// <summary>
		/// The main switch for the messages from stack
		/// </summary>
		/// <param name="messageType">type of the message to be proccessed</param>
		/// <param name="flatmap">the message itself</param>
        private void ProcessMessageAsync(int messageType, FlatmapList flatmap)
        {
			//dump out the message content for debugging 
            if(logMessageBodies)
            {
                StringBuilder sb = new StringBuilder();
                foreach(uint fieldKey in flatmap.Keys)
                {
                    object fieldValueObj = flatmap.Find(fieldKey, 1).dataValue;
                    string fieldValue = fieldValueObj == null ? "<null>" : fieldValueObj.ToString();
                    sb.AppendFormat("Field: {0}({1}) = {2}", fieldKey, ((MsgField) fieldKey).ToString(), fieldValue);
                    sb.AppendLine();
                }

                log.Write(TraceLevel.Info, sb.ToString());
            }

            switch((MsgType)messageType)
            {
                case MsgType.Error: 
                    ProcessError(flatmap);
                    break;

				case MsgType.StartStackAck:
					ProcessStartStackAck(flatmap);
					break;

                case MsgType.RegisterAck:
                    ProcessRegisterAck(flatmap);
                    break;

                case MsgType.SubscribeAck:
                    ProcessSubscribeAck(flatmap);
                    break;

                case MsgType.PublishAck:
                    ProcessPublishAck(flatmap);
                    break;

                case MsgType.Notify:
                    ProcessNotify(flatmap);
                    break;

                case MsgType.SubscriptionTerminated:
                    ProcessSubscriptionTerminated(flatmap);
                    break;

                default:
                    log.Write(TraceLevel.Error, 
                        "Received unknown message type '{0}' from Presence service", messageType);
                    break;
            }
        }

		/// <summary>
		/// It handles error message from stack
		/// </summary>
		/// <param name="flatmap">the message</param>
        private void ProcessError(FlatmapList flatmap)
        {
            Assertion.Check(onError != null, "onError delegate not hooked in Stack proxy");
            
			string message = null;
			try
			{
				message = Convert.ToString(flatmap.Find((uint) MsgField.ResultMsg, 1).dataValue);
			}
			catch
			{
				log.Write(TraceLevel.Error, "Received failure message from Presence service with no reason specified.");
				return;
			}

            string subscriber = null;
            string requestUri = null;
            long resultCode = (long) ResultCodes.Failure;
            try
            {
                subscriber = Convert.ToString(flatmap.Find((uint) MsgField.Subscriber, 1).dataValue);
                requestUri = Convert.ToString(flatmap.Find((uint) MsgField.RequestUri, 1).dataValue);
                resultCode = Convert.ToInt32(flatmap.Find((uint) MsgField.ResultCode, 1).dataValue);
            }
            catch
            {
                //these two fields can be empty
            }

            onError(subscriber, requestUri, message, resultCode);
        }

		/// <summary>
		/// It handles StartStack response from stack
		/// </summary>
		/// <param name="flatmap"></param>
		private void ProcessStartStackAck(FlatmapList flatmap)
		{
			Assertion.Check(onError != null, "onError delegate not hooked in Stack proxy");
            Assertion.Check(onStackStarted != null, "onStackStarted delegate not hooked in stack proxy");

			ResultCodes resultCode = ResultCodes.Failure;
			try
			{
				resultCode = (ResultCodes) Convert.ToInt32(flatmap.Find((uint)MsgField.ResultCode, 1).dataValue);
			}
			catch
			{
				log.Write(TraceLevel.Error, "Received StartStackAck message from Presence service with no ResultCode field.");
				return;
			}

			string message = null;
			if (resultCode == ResultCodes.Failure)	//something went wrong starting up
			{
				try
				{
					message = Convert.ToString(flatmap.Find((uint) MsgField.ResultMsg, 1).dataValue);
				}
				catch
				{
					log.Write(TraceLevel.Error, "Received failure message from Presence service with no reason specified.");
					return;
				}

				onError(null, null, message, (long)resultCode);
			}
			else //the stack started
			{
				//start processing
				this.stackInitialized = true;
                onStackStarted();
			}
		}

		/// <summary>
		/// It handles Register response from stack
		/// </summary>
		/// <param name="flatmap">the message content</param>
        private void ProcessRegisterAck(FlatmapList flatmap)
        {
            Assertion.Check(onRegisterAck != null, "onRegisterAck delegate not hooked in Stack proxy");

            string subscriber;
            try
            {
                subscriber = Convert.ToString(flatmap.Find((uint) MsgField.Subscriber, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received RegisterAck message from Presence service with no subscriber specified.");
                return;
            }
            
            string stackCallId;
			try
			{
				stackCallId = Convert.ToString(flatmap.Find((uint)MsgField.StackCallId, 1).dataValue);
			}
			catch
			{
				log.Write(TraceLevel.Error, "Received RegisterAck message from Presence service with no stack call ID specified.");
				return;
			}

            int result = (int) ResultCodes.Failure;
            try
            {
                result = Convert.ToInt32(flatmap.Find((uint) MsgField.ResultCode, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received RegisterAck message from Presence service with no result code specified.");
                return;
            }

            onRegisterAck(subscriber, stackCallId, result);
        }

		
		/// <summary>
		/// It handles subscribe response from stack
		/// </summary>
		/// <param name="flatmap">the message</param>
		private void ProcessSubscribeAck(FlatmapList flatmap)
        {
            Assertion.Check(onSubscribeAck != null, "onSubscribeAck delegate not hooked in Stack proxy");

            string subscriber;
            try
            {
                subscriber = Convert.ToString(flatmap.Find((uint) MsgField.Subscriber, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received SubscribeAck message from Presence service with no subscriber specified.");
                return;
            }

            string requestUri;
            try
            {
                requestUri = Convert.ToString(flatmap.Find((uint) MsgField.RequestUri, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received SubscribeAck message from Presence service with no requestUri specified.");
                return;
            }

            //string appName;
            //try
            //{
            //    appName = Convert.ToString(flatmap.Find((uint) MsgField.AppName, 1).dataValue);
            //}
            //catch
            //{
            //    log.Write(TraceLevel.Error, "Received SubscribeAck message from Presence service with no appName specified.");
            //    return;
            //}

            string resultMsg;
            try
            {
                resultMsg = Convert.ToString(flatmap.Find((uint) MsgField.ResultMsg, 1).dataValue);
            }
            catch
            {
                //ignore the exception, resultMsg can be missing 
                resultMsg = "";
            }

            string stackCallId;
            try
            {
                stackCallId = Convert.ToString(flatmap.Find((uint) MsgField.StackCallId, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Warning, "Received SubscribeAck message from Presence service with no stack call ID specified.");
                stackCallId = "";
            }


            int result = (int) ResultCodes.Failure;
            try
            {
                result = Convert.ToInt32(flatmap.Find((uint) MsgField.ResultCode, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received RegisterAck message from Presence service with no result code specified.");
                return;
            }

            log.Write(TraceLevel.Verbose, "SubscribeAck contents: subscriber={0}, requestUri={1}, stackCallId={2}, resultCode={3}",
                subscriber, requestUri, stackCallId, result);
            onSubscribeAck(subscriber, requestUri, stackCallId, result, resultMsg);
        }

        /// <summary>
        /// It handles publish response from stack
        /// </summary>
        /// <param name="flatmap">the message</param>
        private void ProcessPublishAck(FlatmapList flatmap)
        {
            Assertion.Check(onPublishAck != null, "onPublishAck delegate not hooked in Stack proxy");

            string stackCallId;
            try
            {
                stackCallId = Convert.ToString(flatmap.Find((uint) MsgField.StackCallId, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received PublishAck message from Presence service with no stack call ID specified.");
                return;
            }

            string requestUri;
            try
            {
                requestUri = Convert.ToString(flatmap.Find((uint) MsgField.Subscriber, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received PublishAck message from Presence service with no requestUri specified.");
                return;
            }

            int result = (int) ResultCodes.Failure;
            try
            {
                result = Convert.ToInt32(flatmap.Find((uint) MsgField.ResultCode, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received RegisterAck message from Presence service with no result code specified.");
                return;
            }

            onPublishAck(requestUri, stackCallId, result);
        }

        /// <summary>
        /// It handles presence notification from stack
        /// </summary>
        /// <param name="flatmap">the message</param>
        private void ProcessNotify(FlatmapList flatmap)
        {
            Assertion.Check(onNotify != null, "onNotify delegate not hooked in Stack proxy");

            string stackCallId;
            try
            {
                stackCallId = Convert.ToString(flatmap.Find((uint) MsgField.StackCallId, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received Notify message from Presence service with no stack call ID specified.");
                return;
            }

            string subscriber;
            try
            {
                subscriber = Convert.ToString(flatmap.Find((uint) MsgField.Subscriber, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received Notify message from Presence service with no subscriber specified.");
                return;
            }

            string requestUri;
            try
            {
                requestUri = Convert.ToString(flatmap.Find((uint) MsgField.RequestUri, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received Notify message from Presence service with no requestUri specified.");
                return;
            }

            //string appName;
            //try
            //{
            //    appName = Convert.ToString(flatmap.Find((uint) MsgField.AppName, 1).dataValue);
            //}
            //catch
            //{
            //    log.Write(TraceLevel.Error, "Received Notify message from Presence service with no appName specified.");
            //    return;
            //}

            string s;
            try
            {
                s = Convert.ToString(flatmap.Find((uint) MsgField.Status, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received Notify message from Presence service with no status specified.");
                return;
            }

            onNotify(subscriber, requestUri, stackCallId, s);
        }

        /// <summary>
        /// It handles presence termination from stack
        /// </summary>
        /// <param name="flatmap">the message</param>
        private void ProcessSubscriptionTerminated(FlatmapList flatmap)
        {
            Assertion.Check(onSubscriptionTerminated != null, "onSubscriptionTerminated delegate not hooked in Stack proxy");

            string subscriber;
            try
            {
                subscriber = Convert.ToString(flatmap.Find((uint) MsgField.Subscriber, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received SubscriptionTerminated message from Presence service with no subscriber specified.");
                return;
            }

            string requestUri;
            try
            {
                requestUri = Convert.ToString(flatmap.Find((uint) MsgField.RequestUri, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received SubscriptionTerminated message from Presence service with no requestUri specified.");
                return;
            }

            long resultCode;
            try
            {
                resultCode = Convert.ToInt32(flatmap.Find((uint) MsgField.ResultCode, 1).dataValue);
            }
            catch
            {
                log.Write(TraceLevel.Error, "Received SubscriptionTerminated message from Presence service with no resultCode specified.");
                return;
            }

            string s;
            try
            {
                s = Convert.ToString(flatmap.Find((uint) MsgField.Reason, 1).dataValue);
            }
            catch
            {
                s = null;
            }

            onSubscriptionTerminated(subscriber, requestUri, s, resultCode);
        }

    #endregion

        #region Service message senders

        /// <summary>
        /// It sends Register message to stack for presence notification.
        /// </summary>
        /// <param name="subscriber">the subscriber uri</param>
        /// <param name="password">the password for the subscriber</param>
        /// <param name="unregister">true if it is to send an unregister message</param>
        /// <returns>true if MakeCall message is sent successfully to the stack. false
        /// otherwise.</returns>
        public bool SendRegister(string subscriber, string password, bool unregister)
        {
            if(this.stackInitialized == false)
                return false;

            StringBuilder sb = new StringBuilder(64);

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int) MsgField.Subscriber, subscriber);
            flatmap.Add((int) MsgField.Password, password);

            if(ipcClient.Write(unregister ? (int) MsgType.Unregister : (int) MsgType.Register, 
                flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Presence service while registering {0}", subscriber);
                return false;
            }
            log.Write(TraceLevel.Verbose,
                "Sent Register subscriber:{0}", subscriber);
            return true;
        }

        /// <summary>
		/// It sends Subscribe message to stack for presence notification.
		/// </summary>
		/// <param name="sub">the subscription information</param>
		/// <param name="di">domain info for the subscription</param>
        /// <param name="unsubscribe">indicate whether it is to unsubscribe or not</param>
		/// <returns>true if MakeCall message is sent successfully to the stack. false otherwise.</returns>
		public bool SendSubscribe(Presence.Subscription sub, SipDomainInfo di)
        {
            if(this.stackInitialized == false)
                return false;

			StringBuilder sb = new StringBuilder(64);
			
			FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int)MsgField.Subscriber, sub.Subscriber);
            flatmap.Add((int) MsgField.Password, sub.Password);
            flatmap.Add((int) MsgField.RequestUri, sub.RequestUri);
            flatmap.Add((int) MsgField.AppName, "dummy value");

            if (sub.CallId != null)
                flatmap.Add((int) MsgField.StackCallId, sub.CallId);

            if(di != null)
            {
                flatmap.Add((int) MsgField.Registrars, di.Registrar.ToString());
                if(di.BackupRegistrar != null)
                    flatmap.Add((int) MsgField.Registrars, di.BackupRegistrar.ToString());
            }

            if(ipcClient.Write(sub.Unsubscribe ? (int) MsgType.Unsubscribe : (int) MsgType.Subscribe, 
                flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Presence service while subscribing {0} -> {1}", 
                    sub.Subscriber, sub.RequestUri);
                return false;
            }

            log.Write(TraceLevel.Verbose,
                "Sent {2} to service: subscriber:{0}, requestUri:{1}", sub.Subscriber, sub.RequestUri, sub.Unsubscribe ? "Unsubscribe" : "Subscribe");
            return true;
        }

        /// <summary>
        /// It sends Publish message to stack for presence notification.
        /// </summary>
        /// <param name="subscriber">the subscriber uri</param>
        /// <param name="password">the password for the subscriber</param>
        /// <param name="requestUri">URI of the presence to be watched</param>
        /// <returns>true if MakeCall message is sent successfully to the stack. false otherwise.</returns>
        public bool SendPublish(string requestUri, string pidf)
        {
            if(this.stackInitialized == false)
                return false;

            StringBuilder sb = new StringBuilder(64);

            FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int) MsgField.Pidf, pidf);
            flatmap.Add((int) MsgField.RequestUri, requestUri);

            if(ipcClient.Write((int) MsgType.Publish, flatmap, Consts.IpcWriteTimeout) == false)
            {
                log.Write(TraceLevel.Error, "Failed to communicate with Presence service while publishing {0}",
                    requestUri);
                return false;
            }

            log.Write(TraceLevel.Verbose,
                "Sent Publish requestUri:{0}, Pidf:{1}", requestUri, pidf);
            return true;
        }

		/// <summary>
		/// It sends StartStack request to stack.
		/// </summary>
		/// <returns>identifies the call to be answered</returns>
		public bool SendStartStack()
		{
			FlatmapList flatmap = new FlatmapList();
            flatmap.Add((int) MsgField.ServiceLogLevel, serviceLogLevel);
            flatmap.Add((int) MsgField.SubscribeExpires, subscribeExpires);
            flatmap.Add((int) MsgField.LogTimingStat, (int) (logTimingStat ? 1 : 0));

			return ipcClient.Write((int)MsgType.StartStack, flatmap, Consts.IpcWriteTimeout);
		}

		/// <summary>
		/// It notifies the stack about parameter change.
		/// 
		/// </summary>
		/// 
		/// <returns>true if the message is sent</returns>
		public bool SendParameterChanged()
		{
			FlatmapList flatmap = new FlatmapList();
			flatmap.Add((int)MsgField.ServiceLogLevel, serviceLogLevel);
			flatmap.Add((int)MsgField.SubscribeExpires, subscribeExpires);
			flatmap.Add((int)MsgField.LogTimingStat, (int) (logTimingStat ? 1 : 0));

			return ipcClient.Write((int)MsgType.ParameterChanged, flatmap, Consts.IpcWriteTimeout);
		}

		#endregion

        #region Private helper methods
        

		#endregion
    }
}
