using System;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Threading;

using RTP=Metreos.Providers.SccpProxy.RtpRelay;
using Metreos.Utilities;

namespace Metreos.Providers.SccpProxy
{
    /// <summary>Used with KillRtpRelay to specify the affected relays</summary>
    [Flags]
    public enum RelayType : uint
    {
        Voice   = 0x01,
        Video   = 0x02,
        All     = 0xFF
    }

	/// <summary>
	/// This class represents an SCCP session between a CCM and SCCP
	/// client, such as a phone.
	/// </summary>
	public class Session : IDisposable
	{
		/// <summary>
		/// Constructor of an SCCP session object.
		/// </summary>
		/// <param name="sid">Station IDentifier. Uniquely identifies this
		/// session within the proxy.</param>
		/// <param name="clientConnection">Connection to the SCCP client.</param>
		/// <param name="connections">List of CCM and client connections.</param>
		/// <param name="provider">Refers back to provider in order to access
		/// methods in its base class.</param>
		public Session(string sid, ClientConnection clientConnection,
			Connections connections, SccpProxyProvider provider,
			Metreos.Utilities.ThreadPool tp)
		{
			Assertion.Check(sid != null,
				"SccpProxyProvider: sid cannot be null");
			Assertion.Check(sid != string.Empty,
				"SccpProxyProvider: sid cannot be empty");
			Assertion.Check(clientConnection != null,
				"SccpProxyProvider: clientConnection cannot be null");
			Assertion.Check(connections != null,
				"SccpProxyProvider: connections cannot be null");
			Assertion.Check(provider != null,
				"SccpProxyProvider: provider cannot be null");

			this.sid = sid;

			ccmConnection = null;	// No connection to the CCM yet.

			// Session is created when the client connection is known.
			this.clientConnection = clientConnection;
			// Add session to connection so connection can refer back to the
			// session that owns it.
			this.clientConnection.MySession = this;

			this.connections = connections;

			processOutgoingMessage = new QueueProcessor( null, tp );

			// We'll need this GUID when we communicate with the application.
			routingGuid = System.Guid.NewGuid().ToString();

			// No message is pending with the application yet.
			pendingMessage = null;

			subscriptions = Hashtable.Synchronized(new Hashtable());

			this.provider = provider;

			needToTearDownLock = new ReaderWriterLock();

			needToTearDown = false;

			disposed = false;
		}

		internal QueueProcessor processOutgoingMessage;

		public void AcquireReaderLock( int timeout )
		{
			needToTearDownLock.AcquireReaderLock( timeout );
		}

		public void ReleaseReaderLock()
		{
			needToTearDownLock.ReleaseReaderLock();
		}

		public void AcquireWriterLock( int timeout )
		{
			needToTearDownLock.AcquireWriterLock( timeout );
		}

		public void ReleaseWriterLock()
		{
			needToTearDownLock.ReleaseWriterLock();
		}

		/// <summary>
		/// Dispose of session resources.
		/// </summary>
		public void Dispose()
		{
			Assertion.Check(connections != null,
				"SccpProxyProvider: connections cannot be null");

			// Check to see if Dispose has already been called.
			if (!disposed)
			{
				disposed = true;

				lock (connections)
				{
					if (clientConnection != null)
					{
						connections.Remove(clientConnection);
					}

					if (ccmConnection != null)
					{
						connections.Remove(ccmConnection);
					}
				}

				// In case they didn't get torn down at the end of the last call,
				// kill any still-active RTP relay threads for this session.
                KillRtpRelay(RelayType.All);

				// Nuke list of messages types to which the app has subscribed.
				subscriptions.Clear();
				subscriptions = null;

				// Nuke the routing GUID since we won't be needing it any more.
				if (routingGuid != null)
				{
					routingGuid = null;
				}

				// This is done last because we may be killing the current
				// thread. In case we are running in one of these threads,
				// check to make sure we kill the other one first.
				if (clientConnection.IsCurrentThread)
				{
					// Should always have a client connection but may not have
					// a CCM connection.
					if (ccmConnection != null)
					{
						ccmConnection.Stop();
					}

					clientConnection.Stop();
				}
				else
				{
					clientConnection.Stop();

					// Should always have a client connection but may not have
					// a CCM connection.
					if (ccmConnection != null)
					{
						ccmConnection.Stop();
					}
				}

				//needToTearDownLock = null;

				//sid = null;
			}

			// This object is cleaned up by the Dispose method. Therefore, we
			// call GC.SupressFinalize to take this object off the finalization
			// queue and prevent finalization code for this object from
			// executing a second time.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Destroy session.
		/// Close both connections (and therefore their underlying sockets),
		/// remove them from the connections table, and kill their read threads.
		/// </summary>
		/// <remarks>
		/// This destructor runs only if the Dispose method does not get
		/// called. It gives the base class the opportunity to finalize.
		/// </remarks>
		~Session()
		{
			Dispose();
		}

		/// <summary>
		/// When an event was last posted to the app.
		/// </summary>
		private long postTime;

		/// <summary>
		/// Read-only property whose value is whether this session is
		/// active--it has not been disposed.
		/// </summary>
		public bool IsActive
		{
			get
			{
				return !disposed;
			}
		}

		/// <summary>
		/// This lock is used to control tearing down the session so that it
		/// is done at "safe" times.
		/// </summary>
		private ReaderWriterLock needToTearDownLock;

		/// <summary>
		/// Read-only property whose value is the reader-writer lock to control
		/// when this session is torn down.
		/// </summary>
		public ReaderWriterLock NeedToTearDownLock
		{
			get
			{
				return needToTearDownLock;
			}
		}

		/// <summary>
		/// Whether there is a need to tear down the session.
		/// </summary>
		private volatile bool needToTearDown;

		/// <summary>
		/// Property whose value is whether to teard down this session at the
		/// most opportune time.
		/// </summary>
		public bool NeedToTearDown
		{
			get
			{
				return needToTearDown;
			}
			set
			{
				needToTearDown = value;
			}
		}

		/// <summary>
		/// Station IDentifier that uniquely identifies the session.
		/// </summary>
		private string sid;

		/// <summary>
		/// Property that has the value of the Station IDentifier that uniquely
		/// identifies the session.
		/// </summary>
		public string Sid
		{
			get
			{
				return sid;
			}
			set
			{
				sid = value;
			}
		}

		/// <summary>
		/// TCP connections with CCMs and SCCP clients.
		/// </summary>
		private Connections connections;

		// Track whether Dispose has been called.
		private bool disposed;

		/// <summary>
		/// Refers back to provider in order to access methods in its base class.
		/// </summary>
		private SccpProxyProvider provider;

        /// <summary>Voice relay proxy</summary>
		private RtpRelay.Relay rtpVoiceRelay = null;
        public RtpRelay.Relay RtpVoiceRelay 
        {
            get { return rtpVoiceRelay; }
            set { rtpVoiceRelay = value; }
        }

        /// <summary>Video relay proxy</summary>
        private RtpRelay.Relay rtpVideoRelay = null;
        public RtpRelay.Relay RtpVideoRelay 
        {
            get { return rtpVideoRelay; }
            set { rtpVideoRelay = value; }
        }

		/// <summary>
		/// Hash table of Message.Type's to which the app has subscribed.
		/// If at least one entry, only those messages are sent to the app as
		/// events; if empty, all messages are sent to the app.
		/// </summary>
		private IDictionary subscriptions;

		/// <summary>
		/// Message whose info the provider has sent to the app. Set to null if
		/// no message pending.
		/// </summary>
		private Message pendingMessage;

		private String pendingMessagePlace;

		/// <summary>
		/// GUID used to associate events and actions sent between provider and
		/// application with this session.
		/// </summary>
		private string routingGuid;

		/// <summary>
		/// Read-only property that has the value of the routing GUID.
		/// </summary>
		public string RoutingGuid
		{
			get
			{
				return routingGuid;
			}
		}

		/// <summary>
		/// Connection to SCCP client. Always present.
		/// </summary>
		private Connection clientConnection;

		/// <summary>
		/// Read-only property whose value is the client connection.
		/// </summary>
		public Connection ClientConnection
		{
			get
			{
				return clientConnection;
			}
		}

		/// <summary>
		/// Connection to CCM. Not initially present.
		/// </summary>
		private Connection ccmConnection;

		/// <summary>
		/// Read-only property whose value is the CCM connection.
		/// </summary>
		public Connection CcmConnection
		{
			get
			{
				return ccmConnection;
			}
		}

		/// <summary>
		/// Add an RtpRelay to this session.
		/// </summary>
		/// <param name="address">Address that uniquely identifies this relay.</param>
		/// <param name="rtpRelay">RtpRelay to add to the session.</param>
		public void AddRtpRelay(RtpRelay.Relay rtpRelay, RelayType type)
		{
            if((type & RelayType.Voice) > 0)
            {
                if(rtpVoiceRelay != null)
                {
                    provider.LogWrite(TraceLevel.Warning,
						"Ses {0}: already has relay {1}; replacing",
                        this, rtpVoiceRelay.RemoteAddr1.ToString());
                }

                this.rtpVoiceRelay = rtpRelay;
            }
            
            if((type & RelayType.Video) > 0)
            {
                if(rtpVideoRelay != null)
                {
                    provider.LogWrite(TraceLevel.Warning,
						"Ses {0}: already has relay {1}; replacing",
                        this, rtpVideoRelay.RemoteAddr1.ToString());
                }

                this.rtpVideoRelay = rtpRelay;
            }
		}

		/// <summary>
		/// Stop the RTP relay and remove our reference to it
		/// </summary>
		/// <remarks>
		/// It won't hurt to invoke this method even if we are not in fact
		/// doing the relaying--we just won't find anything to kill.
		/// </remarks>
		/// <param name="voice">Relay(s) to kill</param>
		public void KillRtpRelay(RelayType type)
		{
            if((type & RelayType.Voice) > 0 && 
                this.rtpVoiceRelay != null)
            {
                this.rtpVoiceRelay.Stop();
                this.rtpVoiceRelay = null;
            }

            if((type & RelayType.Video) > 0 && 
                this.rtpVideoRelay != null)
            {
                this.rtpVideoRelay.Stop();
                this.rtpVideoRelay = null;
            }
		}

		/// <summary>
		/// Return the connection that is the counterpart of the connection
		/// over which the message was sent.
		/// </summary>
		/// <param name="message">Message sent over the connection.</param>
		/// <returns>Counterpart connection.</returns>
		public Connection CounterpartConnection(Message message)
		{
			return CounterpartConnection(message.FromUniqueAddress);
		}

		/// <summary>
		/// Return the connection that is the counterpart of the connection
		/// uniquely identified by the specified address.
		/// </summary>
		/// <param name="address">Address that uniquely identifies the
		/// connection.</param>
		/// <returns>Counterpart connection.</returns>
		public Connection CounterpartConnection(IPEndPoint address)
		{
			Connection counterpartConnection;

			if (address == clientConnection.UniqueAddress)
			{
				counterpartConnection = ccmConnection;
			}
			else if (address == ccmConnection.UniqueAddress)
			{
				counterpartConnection = clientConnection;
			}
			else
			{
				counterpartConnection = null;
			}

			return counterpartConnection;
		}

		/// <summary>
		/// Return the address of the counterpart of the connection
		/// over which the specified message was sent.
		/// </summary>
		/// <param name="message">Message sent over the connection.</param>
		/// <returns>Address of counterpart connection.</returns>
		public IPEndPoint CounterpartAddress(Message message)
		{
			return CounterpartAddress(message.FromUniqueAddress);
		}

		/// <summary>
		/// Return the address of the counterpart of the connection
		/// uniquely identified by the specified address.
		/// </summary>
		/// <param name="address">Address that uniquely identifies the
		/// connection whose counterpart is returned.</param>
		/// <returns>Address of counterpart connection.</returns>
		public IPEndPoint CounterpartAddress(IPEndPoint address)
		{
			IPEndPoint counterpartAddress;

			if (address == clientConnection.UniqueAddress)
			{
				counterpartAddress = ccmConnection.UniqueAddress;
			}
			else if (address == ccmConnection.UniqueAddress)
			{
				counterpartAddress = clientConnection.UniqueAddress;
			}
			else
			{
				counterpartAddress = null;
			}

			return counterpartAddress;
		}

		/// <summary>
		/// Attach CCM connection to this session.
		/// </summary>
		/// <remarks>
		/// Cannot attach a CCM connection if one already attached.
		/// </remarks>
		/// <param name="ccmConnection">CCM connection to attach to the session
		/// object.</param>
		public bool AttachCcmConnection(CcmConnection ccmConnection)
		{
			provider.LogWrite(TraceLevel.Info,
				"Ses {0}: adding {1}",
				this, ccmConnection );
			
			if (this.ccmConnection != null)
			{
				// this is a race condition. the session is being
				// torn down and the new one is already being
				// established. so nuke it.
				return false;
			}

			Assertion.Check(ccmConnection != null,
				"SccpProxyProvider: no ccm connection to attach");
//			if (this.ccmConnection != null)
//				Assertion.Check(this.ccmConnection == null,
//					"SccpProxyProvider: ccm connection, " +
//					/*this.ccmConnection.ToString() + */", already attached");
			// TODO: Don't know why I have to comment the above out.

			this.ccmConnection = ccmConnection;

			// Add session to connection so connection can refer back to the
			// session that owns it.
			this.ccmConnection.MySession = this;
			return true;
		}

		/// <summary>
		/// To identify a session in textual form, return the Station IDentifier.
		/// </summary>
		/// <returns>Station IDentifier.</returns>
		public override string ToString()
		{
			return sid == null || sid == string.Empty ? "?" : sid;
		}

		/// <summary>
		/// Add a pending message.
		/// </summary>
		/// <param name="message">Message to add to the session as a pending
		/// message.</param>
		public void AddPendingMessage(Message message)
		{
			lock (this)
			{
				if (pendingMessage != null)
				{
					provider.LogWrite(TraceLevel.Error,
						"Ses {0}: already has pending message {1} from {2}; overriden",
						this, pendingMessage, pendingMessagePlace);
				}
				pendingMessagePlace = Environment.StackTrace;
				pendingMessage = message;

				// Record when we added this pending message. This is assumed to be
				// coincident with sending an event that is associated with this
				// message up to the app.
				postTime = HPTimer.Now();
			}
		}

		/// <summary>
		/// Read-only property whose value is whether this session has a
		/// pending message.
		/// </summary>
		public bool HasPendingMessage {	get	{ return pendingMessage != null; } }

		/// <summary>
		/// Remove and return the pending message.
		/// </summary>
		/// <returns>(Previously) pending message.</returns>
		public Message RemovePendingMessage()
		{
			lock (this)
			{
				Message message = pendingMessage;

				pendingMessage = null;
				pendingMessagePlace = null;

				if (message != null)
					LogResponseTime(message);
				
				return message;
			}
		}

		/// <summary>
		/// Log elapsed time since pending message was last saved (assumed to
		/// be coincident with last event being posted to app.
		/// </summary>
		/// <param name="message">Name of message that the event represented.</param>
		private void LogResponseTime(Message message)
		{
			string howLong;

			if (postTime == 0)
			{
				howLong = "?";
			}
			else
			{
				howLong = (HPTimer.NsSince( postTime )/1000000).ToString();

				postTime = 0;
			}

			provider.LogWrite(TraceLevel.Info,
				"Ses {0}: handling {1} from app in {2} ms",
				this, message, howLong);
		}

		/// <summary>
		/// Subscribe to the specified list of messages.
		/// </summary>
		/// <param name="subscriptions">Array of string representations of
		/// SCCP message types (Message.Type).</param>
		public void Subscribe(string[] subscriptionStrings)
		{
			Assertion.Check(subscriptionStrings != null,
				"SccpProxyProvider: subscriptions missing");

			subscriptions.Clear();	// Subscriptions are not additive.

			// Convert each string in array to corresponding enum,
			// e.g., "IpPort" -> Message.Type.IpPort, and add to
			// subscription list.
			foreach (string subscription in subscriptionStrings)
			{
				Message.Type messageType;

				if (Message.StringToType(subscription, out messageType))
				{
					// Avoid attempting to add duplicate message type.
					if (subscriptions.Contains(messageType))
					{
						provider.LogWrite(TraceLevel.Warning,
							"Ses {0}: {1} duplicated; ignored",
							this, messageType);
					}
					else
					{
						subscriptions.Add(messageType, null);
					}
				}
				else
				{
					provider.LogWrite(TraceLevel.Warning,
						"Ses {0}: {1} not message type; cannot subscribe to",
						this, subscription);
				}
			}
		}

		/// <summary>
		/// Return whether the app has subscribed to messages of this type.
		/// </summary>
		/// <remarks>
		/// If the app has not specified a subscription list, all message types
		/// are de facto subscribed to.
		/// </remarks>
		/// <param name="messageType">Message whose type to test whether app has
		/// subscribed to it.</param>
		/// <returns>Whether the app has subscribed to the specified message
		/// type.</returns>
		public bool IsSubscribed(Message message)
		{
			return IsSubscribed(message.MessageType);
		}

		/// <summary>
		/// Return whether the app has subscribed to the specified message
		/// type.
		/// </summary>
		/// <remarks>
		/// If the app has not specified a subscription list, all message types
		/// are de facto subscribed to.
		/// </remarks>
		/// <param name="messageType">Message type to test whether app has
		/// subscribed to it.</param>
		/// <returns>Whether the app has subscribed to the specified message
		/// type.</returns>
		public bool IsSubscribed(Message.Type messageType)
		{
			return subscriptions.Contains(messageType);
		}

		/// <summary>
		/// Read-only property whose value is the number of event subscriptions
		/// in the subscriptions hash table.
		/// </summary>
		public int SubscriptionsCount { get { return subscriptions.Count; } }
	}
}
