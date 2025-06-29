using System;
using System.Net;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.Utilities.Selectors;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents a CallManager.
	/// </summary>
	/// <remarks>
	/// This class uses a state machine to maintain a connection with a
	/// specific CallManager.
	/// 
	/// Was called sccpcm in the PSCCP code.
	/// </remarks>
	[StateMachine(ActionPrefix="Handle", StateDefinitionPrefix="Define")]
	internal class CallManager : SccpStateMachine
	{
		/// <summary>
		/// Constructs a CallManager abstraction.
		/// </summary>
		/// <param name="type">Type of CallManager.</param>
		/// <param name="callManagerAddress">CallManager IP address.</param>
		/// <param name="device">Device on which this object is being used.</param>
		/// <param name="log">Object through which log entries are generated.</param>
		/// <param name="selector">Selector that performs socket I/O for
		/// all connections.</param>
		/// <param name="threadPool">Thread pool to offload processing of
		/// selected actions from selector callback.</param>
		internal CallManager(CallManagerType type, IPEndPoint callManagerAddress,
			Device device, LogWriter log, SelectorBase selector,
			Metreos.Utilities.ThreadPool threadPool) :
			base("CMg", device, log, ref control)
		{
			rand = new Random();

			this.callManagerAddress = callManagerAddress;

			// Retry to connect a different number of times depending on
			// CallManager type.
			this.type = type;
			switch (this.type)
			{
				case CallManagerType.Standard:
				case CallManagerType.StandardTftp:
					maxConnectionRetries = callManagerConnectRetries;
					break;

				case CallManagerType.SrstFallback:
					maxConnectionRetries = srstConnectRetries;
					break;

				default:
					maxConnectionRetries = 0;
					break;
			}

			connection = null;
			ackRspRetries = 0;
			timeoutsBeforeRegister = 0;
			shutdown = false;
			keepaliveCount = 0;
			miscTimer = new EventTimer(log);
			keepaliveTimer = new EventTimer(log);
			connectionFactory = new SccpConnectionFactory(this, device, log, selector, threadPool);
		}

		/// <summary>
		/// Constructs just the internal, static state machine.
		/// </summary>
		/// <remarks>
		/// Never reference an object constructed with this constructor--just
		/// instantiate it. Calling this constuctor is optional. Do so if you
		/// are concerned about the little extra time the real constructor will
		/// take to build the internal, static state machine the first time it
		/// is called.
		/// </remarks>
		/// <example>
		/// new CallManager();
		/// </example>
		/// <param name="log">Object through which log entries are generated.</param>
		public CallManager(LogWriter log) : base(log, ref control) { }

		/// <summary>
		/// Object through which StateMachine assures that static state machine
		/// is constructed only once.
		/// </summary>
		private static StateMachineStaticControl control = new StateMachineStaticControl();

		/// <summary>
		/// Generator of random delay before sending first Keepalive after
		/// registration.
		/// </summary>
		private Random rand;

		#region Configuration parameters
		/// <summary>
		/// Whether to log Verbose diagnostics.
		/// </summary>
		private static bool isLogVerbose = false;

		/// <summary>
		/// Whether to log Verbose diagnostics.
		/// </summary>
		internal static bool IsLogVerbose { set { isLogVerbose = value; } }

		/// <summary>
		/// Minimum wait before sending Keepalive after registration in seconds.
		/// </summary>
		private static volatile int minWaitForKeepaliveAfterRegisterSec = 1;

		/// <summary>
		/// Minimum wait before sending Keepalive after registration in seconds.
		/// </summary>
		internal static int MinWaitForKeepaliveAfterRegisterSec { set { minWaitForKeepaliveAfterRegisterSec = value; } }

		/// <summary>
		/// Maximum wait before sending Keepalive after registration in seconds.
		/// </summary>
		private static volatile int maxWaitForKeepaliveAfterRegisterSec = 17;

		/// <summary>
		/// Maximum wait before sending Keepalive after registration in seconds.
		/// </summary>
		internal static int MaxWaitForKeepaliveAfterRegisterSec { set { maxWaitForKeepaliveAfterRegisterSec = value; } }

		/// <summary>
		/// Waiting for RegisterAck Keepalive in milliseconds.
		/// </summary>
		private static volatile int waitingForRegisterAckKeepaliveMs = 30 * 1000;

		/// <summary>
		/// Waiting for RegisterAck Keepalive in milliseconds.
		/// </summary>
		internal static int WaitingForRegisterAckKeepaliveMs
		{
			get { return waitingForRegisterAckKeepaliveMs; }
			set { waitingForRegisterAckKeepaliveMs = value; }
		}

		/// <summary>
		/// Retry TokenRequest in milliseconds.
		/// </summary>
		private static volatile int retryTokenRequestMs = 5 * 1000;

		/// <summary>
		/// Retry TokenRequest in milliseconds.
		/// </summary>
		internal static int RetryTokenRequestMs { set { retryTokenRequestMs = value; } }

		/// <summary>
		/// Number of stuck alarms.
		/// </summary>
		private static volatile int stuckAlarmCount = 6;

		/// <summary>
		/// Number of stuck alarms.
		/// </summary>
		internal static int StuckAlarmCount { set { stuckAlarmCount = value; } }

		/// <summary>
		/// Number of stuck alarms.
		/// </summary>
		private static volatile int callManagerConnectRetries = 2;

		/// <summary>
		/// Number of stuck alarms.
		/// </summary>
		internal static int CallManagerConnectRetries { set { callManagerConnectRetries = value; } }

		/// <summary>
		/// Number of stuck alarms.
		/// </summary>
		private static volatile int srstConnectRetries = 0;

		/// <summary>
		/// Number of stuck alarms.
		/// </summary>
		internal static int SrstConnectRetries { set { srstConnectRetries = value; } }

		/// <summary>
		/// Amount of variance applied to keepalive transmission delay to avoid
		/// all timers synchronizing. Delay still averages out to the keepalive
		/// transmission delay.
		/// </summary>
		/// <remarks>
		/// Value is in percent variance, with normal distribution, average
		/// centered on keepalive delay. For example, if set to 10 and
		/// keepalive delay was set to 60, resulting delay would range between
		/// 50 and 70.
		/// </remarks>
		private static volatile int keepaliveJitterPercent = 20;

		/// <summary>
		/// Amount of variance applied to keepalive transmission delay to avoid
		/// all timers synchronizing. Delay still averages out to the keepalive
		/// transmission delay.
		/// </summary>
		internal static int KeepaliveJitterPercent { set { keepaliveJitterPercent = value; } }
		#endregion

		/// <summary>
		/// Whether to log the transitions for this state machine.
		/// </summary>
		/// <remarks>
		/// This property is just referenced by the parent class to access this
		/// child's static logVerbose setting.
		/// </remarks>
		protected override bool LogTransitions { get { return isLogVerbose; } }

		/// <summary>
		/// High-level CallManager state.
		/// </summary>
		/// <remarks>Maintained exclusively by Device, apparently
		/// solely to determine when to clear the device-data structure,
		/// sappStatusDataInfo. This is cminfo->state in PSCCP, which is for
		/// some reason different from the CallManager states. TBD - Maybe
		/// it can be eliminated in the future.</remarks>
		private volatile HighLevelState_ highLevelState;

		/// <summary>
		/// Property whose value is the high-level CallManager state.
		/// </summary>
		internal HighLevelState_ HighLevelState
		{
			get { return highLevelState; }
			set { highLevelState = value; }
		}

		/// <summary>
		/// Returns whether this CallManager is in the specified state.
		/// </summary>
		/// <param name="state">State for which to test.</param>
		/// <returns>Whether this CallManager is in the specified state.</returns>
		internal bool IsHighLevelState(HighLevelState_ state)
		{
			return highLevelState == state;
		}

		/// <summary>
		/// Enumeration of the high-level CallManager states, not to be
		/// confused with the state-machine states.
		/// </summary>
		/// <remarks>sapp_cm_states_e in PSCCP.</remarks>
		internal enum HighLevelState_
		{
			Closed,
			Connecting,
			Connected,
			Registering,
			Registered,
		}

		/// <summary>
		/// Property whose value is whether this CallManager is
		/// "active"--whether we are actively engaged with this CallManager.
		/// </summary>
		internal bool IsActive
		{
			get
			{
				return IsState(waitingForRegisterResponse) ||
					IsState(registered) ||
					IsState(waitingForUnregisterResponse);
			}
		}

		/// <summary>
		/// Creates SccpConnection objects.
		/// </summary>
		private readonly SccpConnectionFactory connectionFactory;

		/// <summary>
		/// Type of this CallManager.
		/// </summary>
		private readonly CallManagerType type;

		/// <summary>
		/// Connection to CallManager.
		/// </summary>
		private SccpConnection connection;

		/// <summary>
		/// Property whose value is the connection to the actual CallManager.
		/// </summary>
		internal SccpConnection Connection { get { return connection; } }

		/// <summary>
		/// IP address of the actual CallManager that this object represents.
		/// </summary>
		private readonly IPEndPoint callManagerAddress;

		/// <summary>
		/// Property whose value is the CallManager IP address.
		/// </summary>
		internal IPEndPoint Address { get { return callManagerAddress; } }

		/// <summary>
		/// Returns whether this is the CallManager's IP address.
		/// </summary>
		/// <param name="address">IPEndPoint address to which we compare.</param>
		/// <returns>Whether this is the IPEndPoint address.</returns>
		internal bool IsAddress(IPEndPoint address)
		{
			return callManagerAddress.Equals(address);
		}

		/// <summary>
		/// How many times we have attempted to establish a TCP connection to
		/// the CallManager.
		/// </summary>
		private volatile int connectionRetries;

		/// <summary>
		/// The maxiumum number of times we attempt to connect to the
		/// CallManager, after which we give up and declare a TCP error.
		/// </summary>
		private volatile int maxConnectionRetries;

		/// <summary>
		/// Apparently used for a couple of things--counts number of
		/// unregistration retries and keep-alive retries.
		/// </summary>
		private volatile int ackRspRetries;

		/// <summary>
		/// Number of Keepalive timeouts that we can have before we receive a
		/// token. Used for migrating from one CallManager to another.
		/// </summary>
		private volatile int timeoutsBeforeRegister;

		/// <summary>
		/// Whether to disconnect from the CallManager later down the road.
		/// </summary>
		/// <remarks>
		/// The only place this is set to true is in CloseWhileRegistered(),
		/// but that was inexplicably commented out in the original PSCCP code
		/// which effectively makes this useless.
		/// </remarks>
		private volatile bool shutdown;

		/// <summary>
		/// Property whose value is whether to disconnect from the CallManager
		/// later down the road.
		/// </summary>
		internal bool Shutdown { get { return shutdown; } }

		/// <summary>
		/// Keeps track of missing KeepaliveAcks from the CallManager while the
		/// device is waiting for the CallManager to respond to the devices
		/// prior registration request.
		/// </summary>
		private volatile int keepaliveCount;

		/// <summary>
		/// Timer the controls when we send Keepalives to the CallManager.
		/// </summary>
		/// <remarks>Access control not needed here because underlying class
		/// provides it.</remarks>
		private readonly EventTimer keepaliveTimer;

		/// <summary>
		/// Timer used for various purposes.
		/// We're only waiting for one miscellaneous thing at a time.
		/// </summary>
		/// <remarks>Access control not needed here because underlying class
		/// provides it.</remarks>
		private readonly EventTimer miscTimer;

		/// <summary>
		/// CallManager type.
		/// </summary>
		internal enum CallManagerType
		{
			Standard,
			StandardTftp,
			SrstFallback,
		}

		/// <summary>
		/// Type of timer based generally on the context of when the timer is
		/// started.
		/// </summary>
		private enum TimerType
		{
			WaitingForConnectRetry,
			WaitingForKeepaliveAck,
			WaitingForRegisterResponse,
			WaitingForUnregisterResponse,
			Lockout,
			TokenRetry,
		}

		/// <summary>
		/// Various kinds of error events that are sent through the state
		/// machine.
		/// </summary>
		internal enum Error
		{
			DrPrimaryRegisterTimeout,
			CantSendKeepalive,
			CantOpenTcp,
			TcpNak,
			CantSendToken,
			NoKeepaliveAck,
			NoUnregisterAck,
			CloseAllNow,
			TcpClose,
			RegisterReject,
			RegisterTimeout,
		}

		/// <summary>
		/// Property whose value is whether this CallManager is in the lockout
		/// state.
		/// </summary>
		internal bool IsLockout { get { return IsState(lockout); } }

		/// <summary>
		/// Property whose value is whether the device is registered with this
		/// CallManager.
		/// </summary>
		internal bool IsRegistered { get { return IsState(registered); } }

		/// <summary>
		/// Property whose value is whether this CallManager is in the token
		/// state--has received a token from the CallManager.
		/// </summary>
		internal bool IsToken { get { return IsState(token); } }

		/// <summary>
		/// Property whose value is whether this CallManager is idle.
		/// </summary>
		internal bool IsIdle { get { return IsState(idle); } }

		/// <summary>
		/// Property whose value is whether we are registered with this
		/// CallManager or we are waiting for a response to a recent
		/// registration request.
		/// </summary>
		internal bool IsOrWillBePrimary { get { return IsPrimary || IsState(waitingForRegisterResponse); } }

		/// <summary>
		/// Property whose value is whether we are registered to this
		/// CallManager.
		/// </summary>
		/// <remarks>
		/// Presumably only one CallManager is in the registered state at a
		/// time, so if this one is registered, it must be the primary.
		/// </remarks>
		internal bool IsPrimary { get { return IsState(registered); } }

		/// <summary>
		/// Property whose value is whether the device is connected to this
		/// CallManager or has received a token from this CallManager.
		/// </summary>
		internal bool IsConnected { get { return IsJustConnected || IsToken; } }

		/// <summary>
		/// Property whose value is whether the device is "just" connected to
		/// this CallManager.
		/// </summary>
		internal bool IsJustConnected { get { return IsState(connected); } }

		/// <summary>
		/// Property whose value is whether this a secondary CallManager.
		/// </summary>
		/// <remarks>\
		/// I think this means, is this a _candidate_ for being a secondary,
		/// standby CallManager. Carry-over from the original PSCCP logic.
		/// </remarks>
		internal bool IsSecondary { get { return IsConnected; } }

		/// <summary>
		/// Property whose value is whether this device is "registered with
		/// fallback."
		/// </summary>
		private bool Fallback
		{
			set
			{
				device.Discoverer.SetRegisteredWithFallback(value, type);
			}
		}

		#region Finite State Machine

		#region State declarations
		// (No access control for states because once constructed, they are not changed.)
		private static State idle = null;
		private static State waitingForConnectResponse = null;
		private static State connected = null;
		private static State token = null;
		private static State waitingForRegisterResponse = null;
		private static State waitingForConnectRetry = null;
		private static State registered = null;
		private static State waitingForUnregisterResponse = null;
		private static State lockout = null;
		#endregion

		/// <summary>
		/// Types of events that can trigger actions within this state machine.
		/// </summary>
		internal enum EventType
		{
			ConnectRequest,
			Keepalive,
			Close,
			Error,
			DisconnectRequest,
			SendToken,
			ReceiveToken,
			RegisterRequest,
			Registered,
			UnregisterRequest,
			Timeout,
			Lockout,
			ConnectAck,
			ConnectRetry,
			UnregisterAck,
			UnregisterNak,
			TcpEventAck,
			TcpEventNak,	// "Nak," from the PSCCP code, apparently just means non-specific TCP error.
			TcpEventOpen,
			TcpEventClose,
			TcpEventReceive,
			RegisterAck,
			RegisterNak,
			KeepaliveAck,
			Reset,
			Connected,
			TokenReject,
			TokenRetryTimeout,
			LockoutTimeout,
			KeepaliveTimeout,
			WaitingForRegisterResponseTimeout,
		}

		#region Action declarations

        #pragma warning disable 1717  // Suppress "Assignment made to same variable" warning
        
        private static ActionDelegate ConnectedDisconnectRequest = null;
		private static ActionDelegate ConnectedLockout = null;
		private static ActionDelegate ConnectedReceiveToken = null;
		private static ActionDelegate ConnectedRegisterRequest = null;
		private static ActionDelegate ConnectedSendToken = null;
		private static ActionDelegate ConnectedTimeout = null;
		private static ActionDelegate ConnectedKeepaliveAck = null;
		private static ActionDelegate IdleConnectRequest = null;
		private static ActionDelegate RegisteredClose = null;
		private static ActionDelegate RegisteredError = null;
		private static ActionDelegate RegisteredLockout = null;
		private static ActionDelegate RegisteredTimeout = RegisteredTimeout;	// Don't know why not used.
		private static ActionDelegate TokenUnregisterNak = null;
		private static ActionDelegate UnregisterRequest = null;
		private static ActionDelegate WaitingForConnectResponseConnectAck = null;
		private static ActionDelegate WaitingForConnectResponseConnectRetry = null;
		private static ActionDelegate WaitingForConnectResponseClose = null;
		private static ActionDelegate WaitingForConnectResponseError = null;
		private static ActionDelegate WaitingForConnectResponseTimeout = null;
		private static ActionDelegate WaitingForConnectRetryConnectRequest = null;
		private static ActionDelegate WaitingForConnectRetryTimeout = null;
		private static ActionDelegate WaitingForRegisterResponseRegisterAck = null;
		private static ActionDelegate WaitingForRegisterResponseRegisterNak = null;
		private static ActionDelegate WaitingForRegisterResponseKeepaliveAck = null;
		private static ActionDelegate WaitingForRegisterResponseTimeout = WaitingForRegisterResponseTimeout;	// Don't know why not used.
		private static ActionDelegate WaitingForRegisterResponseWaitingForRegisterResponseTimeout = null;
		private static ActionDelegate WaitingForUnregisterResponseClose = null;
		private static ActionDelegate WaitingForUnregisterResponseError = null;
		private static ActionDelegate WaitingForUnregisterResponseTimeout = null;
		private static ActionDelegate WaitingForUnregisterResponseUnregisterAck = null;
		private static ActionDelegate WaitingForUnregisterResponseUnregisterNak = null;
		private static ActionDelegate TcpEventOpen = null;
		private static ActionDelegate TcpEventClose = null;
		private static ActionDelegate TcpConnectFailure = null;
		private static ActionDelegate UnexpectedTcpNack = null;
		private static ActionDelegate Keepalive = null;
		private static ActionDelegate LockoutTimeoutKeepalive = null;
		private static ActionDelegate LockoutTimeoutLockout = null;
		private static ActionDelegate LockoutKeepaliveAck = null;
		private static ActionDelegate Registered = null;
		private static ActionDelegate TokenReject = null;
		private static ActionDelegate TokenRetryTimeout = null;

        #pragma warning restore 1717
        
        #endregion

        #region State-definition methods
        /// <summary>
		/// Defines the idle state where the CallManager has just been
		/// constructed and essentially nothing else has happened yet.
		/// </summary>
		/// <remarks>
		/// We are mainly waiting for a request from the consumer to connect to
		/// the CallManager.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State(Initial=true)]
		private static void DefineIdle(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.ConnectRequest,		IdleConnectRequest,			waitingForConnectResponse);
			state.Add(EventType.TcpEventClose);				// Ignore
			state.Add(EventType.TcpEventNak);				// Ignore--CallManager probably "forcibly" closed socket
			state.Add(EventType.UnregisterAck);				// Ignore
			state.Add(EventType.KeepaliveAck);				// Ignore--probably just a straggler during shutdown
			state.Add(EventType.DisconnectRequest);			// We're already disconnected, so ignore
			state.Add(EventType.Error);						// We're disconnected, so ignore; nothing we can do about it now
		}

		/// <summary>
		/// Defines the waiting-for-connect-response state where we are
		/// waiting for a response to our having initiated a connection to the
		/// CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineWaitingForConnectResponse(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.ConnectAck,			WaitingForConnectResponseConnectAck,
																				connected);
			state.Add(EventType.ConnectRetry,		WaitingForConnectResponseConnectRetry,
																				waitingForConnectRetry);
			state.Add(EventType.UnregisterAck);				// Ignore--probably internally generated UnregisterAck
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		TcpConnectFailure);
			state.Add(EventType.Timeout,			WaitingForConnectResponseTimeout,
																				idle);
			state.Add(EventType.Close,				WaitingForConnectResponseClose,
																				idle);
			state.Add(EventType.Error,				WaitingForConnectResponseError,
																				idle);
		}

		/// <summary>
		/// Defines the connected state where have established a
		/// connection to the CallManager and are now ready to register with
		/// it.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineConnected(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.SendToken,			ConnectedSendToken);
			state.Add(EventType.ReceiveToken,		ConnectedReceiveToken,		token);
			state.Add(EventType.DisconnectRequest,	ConnectedDisconnectRequest,	idle);
			state.Add(EventType.RegisterRequest,	ConnectedRegisterRequest,	waitingForRegisterResponse);
			state.Add(EventType.KeepaliveAck,		ConnectedKeepaliveAck);
			state.Add(EventType.Lockout,			ConnectedLockout,			lockout);
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		UnexpectedTcpNack);
			state.Add(EventType.TokenReject,		TokenReject);
			state.Add(EventType.Timeout,			ConnectedTimeout);
			state.Add(EventType.TokenRetryTimeout,	TokenRetryTimeout);
			state.Add(EventType.Close,				WaitingForConnectResponseClose,
																				idle);
			state.Add(EventType.Error,				WaitingForConnectResponseError,
																				idle);
			state.Add(EventType.Keepalive,			Keepalive);
			state.Add(EventType.UnregisterAck);				// We're merely connected now, so ignore
		}

		/// <summary>
		/// Defines the token state where we have "obtained the token"
		/// from the CallManager, granting permission to register with it, and
		/// are now ready to register.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineToken(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.DisconnectRequest,	ConnectedDisconnectRequest,	idle);
			state.Add(EventType.RegisterRequest,	ConnectedRegisterRequest,	waitingForRegisterResponse);
			state.Add(EventType.UnregisterNak,		TokenUnregisterNak,			connected);
			state.Add(EventType.KeepaliveAck,		ConnectedKeepaliveAck);
			state.Add(EventType.Lockout,			ConnectedLockout,			lockout);
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		UnexpectedTcpNack);
			state.Add(EventType.Timeout,			ConnectedTimeout);
			state.Add(EventType.Close,				WaitingForConnectResponseClose,
																				idle);
			state.Add(EventType.Error,				WaitingForConnectResponseError,
																				idle);
			state.Add(EventType.Keepalive,			Keepalive);
		}

		/// <summary>
		/// Defines the waiting-for-register-response state where we are
		/// waiting for a response to our prior registration request.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineWaitingForRegisterResponse(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.UnregisterRequest,	UnregisterRequest,			waitingForUnregisterResponse);
			state.Add(EventType.RegisterAck,		WaitingForRegisterResponseRegisterAck,
																				registered);
			state.Add(EventType.RegisterNak,		WaitingForRegisterResponseRegisterNak);
			state.Add(EventType.KeepaliveAck,		WaitingForRegisterResponseKeepaliveAck);
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		UnexpectedTcpNack);
			state.Add(EventType.Timeout,			ConnectedTimeout);
			state.Add(EventType.WaitingForRegisterResponseTimeout,
													WaitingForRegisterResponseWaitingForRegisterResponseTimeout);
			state.Add(EventType.Close,				WaitingForConnectResponseClose,
																				idle);
			state.Add(EventType.Error,				WaitingForConnectResponseError,
																				idle);
			state.Add(EventType.Keepalive,			Keepalive);
		}

		/// <summary>
		/// Defines the waiting-for-connect-retry state where we are
		/// waiting for a timer to expire before initiating a connection to the
		/// CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineWaitingForConnectRetry(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.ConnectRequest,		WaitingForConnectRetryConnectRequest,
																				waitingForConnectResponse);
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		UnexpectedTcpNack);
			state.Add(EventType.Timeout,			WaitingForConnectRetryTimeout);
			state.Add(EventType.Close,				WaitingForConnectResponseClose,
																				idle);
			state.Add(EventType.Error,				WaitingForConnectResponseError,
																				idle);
			state.Add(EventType.Keepalive,			Keepalive);
		}

		/// <summary>
		/// Defines the registered state where the device is registered
		/// to this CallManager.
		/// </summary>
		/// <remarks>
		/// This is the state we want to be in most of the time.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineRegistered(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.UnregisterRequest,	UnregisterRequest,			waitingForUnregisterResponse);
			state.Add(EventType.Registered,			Registered);
			state.Add(EventType.Lockout,			RegisteredLockout,			lockout);
			state.Add(EventType.KeepaliveAck,		ConnectedKeepaliveAck);
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		UnexpectedTcpNack);
			state.Add(EventType.Timeout,			ConnectedTimeout);
			state.Add(EventType.Close,				RegisteredClose,			connected);	// (on the way to being idle)
			state.Add(EventType.Error,				RegisteredError,			connected);	// (on the way to being idle)
			state.Add(EventType.Keepalive,			Keepalive);
		}

		/// <summary>
		/// Defines the waiting-for-unregister-response state where the
		/// device is waiting for the CallManager to respond to its prior
		/// unregistration request.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineWaitingForUnregisterResponse(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.UnregisterAck,		WaitingForUnregisterResponseUnregisterAck,
																				connected);
			state.Add(EventType.UnregisterNak,		WaitingForUnregisterResponseUnregisterNak,
																				registered);
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		UnexpectedTcpNack);
			state.Add(EventType.Timeout,			WaitingForUnregisterResponseTimeout);
			state.Add(EventType.Close,				WaitingForUnregisterResponseClose,
																				idle);
			state.Add(EventType.Error,				WaitingForUnregisterResponseError,
																				idle);
			state.Add(EventType.Keepalive,			Keepalive);
		}

		/// <summary>
		/// Defines the lockout state where the device has basically
		/// given up on this CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineLockout(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.DisconnectRequest,	ConnectedDisconnectRequest,	idle);
			state.Add(EventType.UnregisterAck);
			state.Add(EventType.KeepaliveAck,		LockoutKeepaliveAck,		connected);
			state.Add(EventType.TcpEventOpen,		TcpEventOpen);
			state.Add(EventType.TcpEventClose,		TcpEventClose);
			state.Add(EventType.TcpEventNak,		UnexpectedTcpNack);
			state.Add(EventType.KeepaliveTimeout,	LockoutTimeoutKeepalive);
			state.Add(EventType.LockoutTimeout,		LockoutTimeoutLockout);
			state.Add(EventType.Close,				WaitingForConnectResponseClose,
																				idle);
			state.Add(EventType.Error,				WaitingForConnectResponseError,
																				idle);
			state.Add(EventType.Keepalive,			Keepalive);
		}

		/// <summary>
		/// Disconnects from the CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnectedDisconnectRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.Disconnect();

			this_.Reset();

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));
		}

		/// <summary>
		/// Notifies the Discovery object that lockout has occurred.
		/// </summary>
		/// <remarks>
		/// The CallManager has missed responding with a KeepaliveAck for three
		/// intervals.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnectedLockout(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.ackRspRetries = 0;
			this_.timeoutsBeforeRegister = -1;

			// Start a timer to supervise the lockout. We keep the
			// connection up and send Keepalives until this timeout.
			this_.StartMiscTimer();
			this_.StartKeepaliveTimer();

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));
		}

		/// <summary>
		/// Notifies Discovery that a token has been received.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnectedReceiveToken(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.timeoutsBeforeRegister = -1;

			// Make sure the waitingForRetryTimeout is cancelled.
			this_.miscTimer.Stop();

			this_.device.Discoverer.AlarmCondition = Discovery.Alarm.LastFailback;

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));
		}

		/// <summary>
		/// Begins registration procedures.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnectedRegisterRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.ackRspRetries = 0;
			this_.timeoutsBeforeRegister = -1;
			this_.miscTimer.Stop();

			// Connected - go ahead and fire off the registrar.
			followupEvents.Enqueue(new Event((int)Registration.EventType.RegisterRequest, this_.device.Registrar));

			// Start timer to wait for a RegisterAck.
			this_.StartMiscTimer();
		}

		/// <summary>
		/// Starts the token request processing. Requests a token from the
		/// CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnectedSendToken(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			Sid sid = new Sid(this_.device.MacAddress);
			IPAddress address = this_.device.Discoverer.ClientOverrideAddress != null ?
				this_.device.Discoverer.ClientOverrideAddress :
				this_.LocalEndPoint.Address;

			if (this_.Send(new RegisterTokenReq(sid, address,
				this_.device.Discoverer.DeviceType)))
			{
				// Set the number of Keepalive timeouts that we can have before
				// we receive a token. If we have too many timeouts then we
				// just assume that we have a token and try to register
				// with the CallManager. Add 1 since we don't know how much
				// longer we have before the next timeout.
				this_.timeoutsBeforeRegister =
					Discovery.KeepaliveTimeoutsBeforeNoTokenRegister + 1;
			}
			else
			{
				followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.CantSendToken), this_));
			}
		}

		/// <summary>
		/// Sends a Keepalive and starts another timer to wait for the
		/// KeepaliveAck because a timeout has occurred.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnectedTimeout(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.CheckStuck();

			// Increment the retry count since we did not receive a
			// KeepaliveAck.
			++this_.ackRspRetries;

			this_.LogVerbose("CMg: {0}: retry: {1}; while connected",
				this_.connection, this_.ackRspRetries);

			// Mark this CallManager as bad if we have missed too many KeepaliveAcks.
			if (this_.ackRspRetries > Discovery.AckRetriesBeforeLockout)
			{
				this_.log.Write(TraceLevel.Warning,
					"CMg: {0}: give up on KeepaliveAck: {1}; while connected",
					this_.connection, this_.ackRspRetries);

				// Bomb the CallManager object if this was the primary or we
				// were trying to make it the primary, otherwise lockout.
				if (this_.IsState(waitingForRegisterResponse))
				{
					WaitingForRegisterResponseWaitingForRegisterResponseTimeout(this_, event_, ref followupEvents);
				}
				else if (this_.IsState(registered))
				{
					followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.NoKeepaliveAck), this_));
				}
				else
				{
					followupEvents.Enqueue(new Event((int)EventType.Lockout, this_));
				}
			}
			else
			{
				// Let's try again.
				if (!this_.Send(new Keepalive()))
				{
					this_.log.Write(TraceLevel.Warning,
						"CMg: {0}: could not re-send Keepalive: {1}; while connected",
						this_.connection, this_.ackRspRetries);

					followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.CantSendKeepalive), this_));
				}
				else
				{
					if (this_.ackRspRetries > 1)
					{
						this_.log.Write(TraceLevel.Warning,
							"CMg: {0}: re-send Keepalive: {1}; while connected",
							this_.connection, this_.ackRspRetries);
					}

					// Start timer to wait for the KeepaliveAck.
					this_.StartKeepaliveTimer();

					// Simulate that a Keepalive was received. The CallManager
					// does not respond to a Keepalive request unless the
					// client is registered. This CallManager is only
					// connected and not registered, therefore, we just pretend
					// that an ack was received to keep the state machine happy.

					// test code
					// comment out the push below to simulate KeepaliveAck failures.

					// This is all that needs to be done for CallManagers in
					// the registered state.
					if (!this_.IsState(registered))
					{
						followupEvents.Enqueue(new Event((int)EventType.KeepaliveAck, this_));

						// Track the number of timeouts if we are trying to
						// receive a token. If we have too many timeouts then
						// we just assume that we have a token and try to
						// register with the CallManager. Add 1 since we don't
						// know how much longer we have before the next timeout.
						if (this_.timeoutsBeforeRegister != -1)
						{
							--this_.timeoutsBeforeRegister;

							// Check if the CallManager is having problems
							// while we were trying to request a token.
							if (this_.timeoutsBeforeRegister == 0)
							{
								// Maybe the CallManager is overloaded right
								// now. Simulate that a token was received and
								// see if that helps.
								this_.timeoutsBeforeRegister = -1;

								followupEvents.Enqueue(new Event((int)EventType.ReceiveToken, this_));
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Clears the retry count and waits for timeout to send another
		/// Keepalive upon receipt of a KeepaliveAck.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnectedKeepaliveAck(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Clear the retry count.
			this_.ackRspRetries = 0;
		}

		/// <summary>
		/// Connects to a CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleIdleConnectRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.connectionRetries = 0;
			this_.ackRspRetries = 0;

			// Close existing connections.
			this_.Disconnect();

			if (!this_.Connect())
			{
				this_.log.Write(TraceLevel.Error, "CMg: {0}: unable to connect", this_.connection);

				followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.CantOpenTcp), this_));
			}
		}

		/// <summary>
		/// Closes CallManager while registered. Unregister from CallManager and
		/// disconnect.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegisteredClose(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.log.Write(TraceLevel.Verbose,
				"CMg: {0}: close while registered", this_.connection);

			this_.CloseWhileRegistered(Error.CloseAllNow, ref followupEvents);
		}

		/// <summary>
		/// Handles error condition. Unregister from CallManager and disconnect.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Error.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegisteredError(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;
			ErrorEvent message = event_.EventMessage as ErrorEvent;

			this_.log.Write(TraceLevel.Error, "CMg: {0}: error {1} while registered",
				this_.connection, message.Error);

			this_.CloseWhileRegistered(message.Error, ref followupEvents);
		}

		/// <summary>
		/// Performs lockout procedures.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegisteredLockout(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.miscTimer.Stop();

			this_.Fallback = false;

			this_.device.Discoverer.AlarmCondition = Discovery.Alarm.LastKeepaliveTimeout;

			// Let the application know that we are not connected to a primary CallManager.
			this_.DeviceStatus(Device.Status.CallManagerDown);

			// Unregister from the CallManager, do not wait for the ACK.
			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterRequest,
				this_.device.Registrar));

			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterAck,
				new UnregisterAck(UnregisterAck.Status.Ok), this_.device.Registrar));

			// Don't really care if we get a response to the unregisterRequest,
			// so let's just issue one now to keep the CallMachine state
			// machine happy.
			followupEvents.Enqueue(new Event((int)EventType.UnregisterAck, this_));

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));
		}

		/// <summary>
		/// Handles timeout. Send Keepalive and wait for KeepaliveAck.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegisteredTimeout(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			if (this_.Send(new Keepalive()))
			{
				// Start timer to wait for the KeepaliveAck.
				this_.StartKeepaliveTimer();
			}
			else
			{
				followupEvents.Enqueue(new Event((int)EventType.Error,
					new ErrorEvent(Error.CantSendKeepalive), this_));
			}
		}

		/// <summary>
		/// Handles UnregisterAck while holding token.
		/// </summary>
		/// <remarks>
		/// Move CallManager to connected state. This happens when fallback
		/// procedures have been initiated and the CallManager we tried to
		/// unregister does not unregister. That CallManager sends unregNak
		/// to this CallManager object to move it to the connected state. This
		/// ensures that everything is back to the way it was before the
		/// fallback and allow Discovery to start over.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTokenUnregisterNak(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			// Do nothing.
		}

		/// <summary>
		/// Handles unregister request from Discovery. Push UnregisterRegister
		/// to Registration and wait for response.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleUnregisterRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.ackRspRetries = 0;

			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterRequest,
				this_.device.Registrar));

			this_.StartMiscTimer();
		}

		/// <summary>
		/// Handles the response for a successful socket connect. Start the
		/// Keepalive timers and notify Discovery of the successful connection.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForConnectResponseConnectAck(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Start timer to wait for the KeepaliveAck. This also cancels the
			// timer that was waiting for the ConnectAck -
			// waitingForConnectResponseTimeout. The KeepaliveAck was reused
			// during this time so that the misc timer would be available for
			// user if connectRetry was started.
			this_.StartKeepaliveTimer();

			this_.LogVerbose("CMg: {0} connected", this_.connection);

			this_.DeviceStatus(Device.Status.CallManagerConnected);

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));
		}

		/// <summary>
		/// Starts retry procedures by starting a short timer to retry the
		/// connect. The socket connect to a CallManager has failed.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForConnectResponseConnectRetry(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Start a timer so we can retry the connect. We need to wait
			// before we retry.
			this_.StartMiscTimer();
		}

		/// <summary>
		/// Closes CallManager while waiting for a connect response from the
		/// CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForConnectResponseClose(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.log.Write(TraceLevel.Verbose,
				"CMg: {0}: close while waiting for connect response",
				this_.connection);

			this_.CloseWhileWaitingForConnectResponse(ref followupEvents);
		}

		/// <summary>
		/// Handles an error while waiting for a connect response from the
		/// CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Error message.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForConnectResponseError(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;
			ErrorEvent message = event_.EventMessage as ErrorEvent;

			this_.log.Write(TraceLevel.Error,
				"CMg: {0}: error {1} in intermediate state on {2}",
				this_, message.Error, this_.connection);

			this_.CloseWhileWaitingForConnectResponse(ref followupEvents);
		}

		/// <summary>
		/// Disconnects the socket and notifies Discovery because the socket
		/// connect to a CallManager has timed out.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForConnectResponseTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// No reply from the other side, so let's just go to idle and let
			// the Discovery object fix things.
			this_.Disconnect();

			this_.Reset();

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));
		}

		/// <summary>
		/// Retries connect to a CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForConnectRetryConnectRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.ackRspRetries = 0;

			if (!this_.device.Discoverer.HaveActiveCallManager())
			{
				this_.device.Discoverer.AlarmCondition =
					Discovery.Alarm.LastPhoneAbort;
			}

			this_.Disconnect();

			if (!this_.Connect())
			{
				this_.log.Write(TraceLevel.Error, "CMg: {0}; unable to connect", this_.connection);

				followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.CantOpenTcp), this_));
			}
		}

		/// <summary>
		/// Proceeds with a connect retry because a timeout has occurred while
		/// waiting to retry a socket connect.
		/// </summary>
		/// <remarks>
		/// This method can do what HandleWaitingForConnectRetryConnectReq does
		/// instead of sending a ConnectRequest event to get to it.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForConnectRetryTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			followupEvents.Enqueue(new Event((int)EventType.ConnectRequest, this_));
		}

		/// <summary>
		/// Handles RegisterAck while waiting for registerAck. Copy the
		/// relevant register information and notify Discovery.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for RegisterAck.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForRegisterResponseRegisterAck(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;
			RegisterAck message = event_.EventMessage as RegisterAck;

			// Stop the waitingForRegisterResponseTimeout.
			this_.miscTimer.Stop();

			// Reset the ack counter since we received an ack.
			this_.ackRspRetries = 0;
			this_.keepaliveCount = 0;

			this_.Fallback = true;

			// Convert Keepalive seconds to milliseconds.
			this_.device.Discoverer.PrimaryKeepaliveTimeout =
				(int)message.keepaliveInterval1 * 1000;
			this_.device.Discoverer.SecondaryKeepaliveTimeout =
				(int)message.keepaliveInterval2 * 1000;

			// Randomize first timeout. We already started a Keepalive timer
			// when the connection was first connected, but that one is
			// canceled when this timeout is set.
			int delayMs = this_.rand.Next(minWaitForKeepaliveAfterRegisterSec * 1000,
				maxWaitForKeepaliveAfterRegisterSec * 1000);
			this_.keepaliveTimer.Start(delayMs, this_,
				(int)TimerType.WaitingForKeepaliveAck);

			this_.LogVerbose("CMg: {0} registered, first Keepalive in {1}ms",
				this_.connection, delayMs);

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));

			// Notify the application that this CallManager is registered.
			this_.DeviceStatus(Device.Status.CallManagerRegistered);
		}

		/// <summary>
		/// Handles RegisterNak while waiting for RegisterAck. Issue error to
		/// cleanup CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for RegisterNak</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForRegisterResponseRegisterNak(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;
			RegisterReject message = event_.EventMessage as RegisterReject;

			// Stop the WaitingForRegisterResponseTimeout
			this_.miscTimer.Stop();

			this_.device.Discoverer.Reject = true;

			this_.device.Discoverer.AlarmCondition = Discovery.Alarm.LastRegisterReject;

			followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.RegisterReject), this_));
		}

		/// <summary>
		/// Handles KeepaliveAck.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForRegisterResponseKeepaliveAck(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Might need to scale back the Keepalive timeout if we are still
			// set to the minimum default.
			if (this_.device.Discoverer.SecondaryKeepaliveTimeout ==
				Discovery.DefaultKeepaliveMs)
			{
				// This is the first KeepaliveAck returned and before the
				// RegisterAck was received, so let's scale back to the
				// WaitingForRegisterAckTimeout so that we do not pound the
				// CallManager.
				this_.device.Discoverer.SecondaryKeepaliveTimeout =
					waitingForRegisterAckKeepaliveMs;
			}

			// ACK received, reset the counter.
			this_.ackRspRetries = 0;
		}

		/// <summary>
		/// Handles timeout for a KeepaliveAck.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForRegisterResponseTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			if (this_.Send(new Keepalive()))
			{
				// Start timer to wait for the KeepaliveAck.
				this_.StartKeepaliveTimer();
			}
			else
			{
				followupEvents.Enqueue(new Event((int)EventType.Error,
					new ErrorEvent(Error.CantSendKeepalive), this_));
			}

			// Check if the CallManager is stuck. Send an alarm for every N
			// Keepalives that the CallManager does not respond to a register.
			this_.CheckStuck();
		}

		/// <summary>
		/// Timeout while waiting for RegisterRequest. Unregister Registration
		/// and error the CallManager to cleanup.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForRegisterResponseWaitingForRegisterResponseTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Unregister the Registration to clean it up.
			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterRequest,
				this_.device.Registrar));

			// Trigger UnregisterAck because we probably will not receive one
			// from the CallManager since we never registered with it.
			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterAck,
				new UnregisterAck(UnregisterAck.Status.Ok), this_.device.Registrar));

			this_.device.Discoverer.AlarmCondition =
				Discovery.Alarm.LastCallManagerAbortTcp;

			followupEvents.Enqueue(new Event((int)EventType.Error,
				new ErrorEvent(Error.RegisterTimeout), this_));
		}

		/// <summary>
		/// Closes CallManager while waiting for response from CallManager to our
		/// Unregister message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForUnregisterResponseClose(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.log.Write(TraceLevel.Verbose,
				"CMg: {0}: close while waiting for unregister response",
				this_.connection);

			this_.CloseWhileWaitingForUnregisterResponse(ref followupEvents);
		}

		/// <summary>
		/// Handles error while waiting for response from CallManager to our
		/// Unregister message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Error.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForUnregisterResponseError(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			ErrorEvent message = event_.EventMessage as ErrorEvent;

			this_.log.Write(TraceLevel.Error,
				"CMg: {0}: error {1} while waiting for unregister response",
				this_.connection, message.Error);

			this_.CloseWhileWaitingForUnregisterResponse(ref followupEvents);
		}

		/// <summary>
		/// Handles timeout. Check ack retry.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForUnregisterResponseTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Increment the retry count, since we did not receive a
			// KeepaliveAck.
			++this_.ackRspRetries;

			this_.LogVerbose("CMg: {0}: retry: {1}; waiting for unregister-response",
				this_.connection, this_.ackRspRetries);

			// Check for max retries.
			if (this_.ackRspRetries >= Discovery.UnregisterAckRetries)
			{
				followupEvents.Enqueue(new Event((int)EventType.Error,
					new ErrorEvent(Error.NoUnregisterAck), this_));
			}
			else
			{
				// Resend the unregister.
				followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterRequest,
					this_.device.Registrar));
				this_.StartMiscTimer();
			}
		}

		/// <summary>
		/// Handles UnregisterAck. Set fallback and notify the application that
		/// the CallManager is down.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForUnregisterResponseUnregisterAck(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.Fallback = false;

			// Let the application know that we are not connected to a primary
			// CallManager.
			this_.DeviceStatus(Device.Status.CallManagerDown);

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, this_.device.Discoverer));
		}

		/// <summary>
		/// Handles unregisterNak. Go back to the registered state and let
		/// Discovery timeout to fix things.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWaitingForUnregisterResponseUnregisterNak(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// If we receive an UnregisterNak, we go back to the registered
			// state and let the Discovery state machine timeout and move to
			// the secondary optimize. Eventually, we detect a better primary
			// available again and try the unregister again.

			// sam not sure if this is right
			// Check if we were in the process of falling back to another
			// CallManager. This is the case if we have a standby with a token.
			// We cannot unregister this CallManager, so we need to go back to
			// the token CallManager and push it to the connected state. This
			// puts everything back to how it was before we started the
			// fallback procedures. Discovery can then start the whole process
			// over again when it times out.
			CallManager standbyCallManager;
			if (this_.device.Discoverer.HaveSecondaryCallManager(
				out standbyCallManager))
			{
				if (standbyCallManager.IsToken)
				{
					followupEvents.Enqueue(new Event((int)EventType.UnregisterNak,
						standbyCallManager));
				}
			}
		}

		/// <summary>
		/// Processes TCP open event from the application. This method
		/// translates the TCP event into a more usable CallManager event.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTcpEventOpen(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			followupEvents.Enqueue(new Event((int)EventType.ConnectAck, this_));
		}

		/// <summary>
		/// Processes TCP close events from the application. This method
		/// translates the TCP event into a more usable CallManager event.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTcpEventClose(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.TcpClose), this_));
		}

		/// <summary>
		/// Processes TCP Nack events while attempting to establish a TCP
		/// connection. This method translates the TCP event into a more
		/// usable CallManager event.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTcpConnectFailure(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			if (this_.connectionRetries < this_.maxConnectionRetries)
			{
				++this_.connectionRetries;
				followupEvents.Enqueue(new Event((int)EventType.ConnectRetry, this_));
			}
			else
			{
				followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.TcpNak), this_));
			}
		}

		/// <summary>
		/// Processes unexpected TCP Nack events. This method
		/// translates the TCP event into a more usable CallManager event.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleUnexpectedTcpNack(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Tell consumer that calls are being released then remove the
			// calls from the stack. (Use WriterLock because we are going to
			// Clear the collection.)
			bool useCookie;
			LockCookie cookie = this_.device.Calls.WriterLock(out useCookie);
			try
			{
				foreach (Call call in this_.device.Calls)
				{
					this_.device.PostReleaseComplete(
						new ReleaseComplete((uint)call.LineNumber,
						ReleaseComplete.Cause.NotConnected));
				}
				this_.device.Calls.Clear();
			}
			finally
			{
				this_.device.Calls.WriterUnlock(cookie, useCookie);
			}

			followupEvents.Enqueue(new Event((int)EventType.Error, new ErrorEvent(Error.TcpNak), this_));
		}

		/// <summary>
		/// Replies with a KeepaliveAck.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleKeepalive(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			this_.Send(new KeepaliveAck());
		}

		/// <summary>
		/// Handles Keepalive timeout. Send Keepalive and start timer to wait
		/// for KeepaliveAck.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleLockoutTimeoutKeepalive(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			if (this_.Send(new Keepalive()))
			{
				// Start timer to wait for the KeepaliveAck.
				this_.StartKeepaliveTimer();
			}
			else
			{
				followupEvents.Enqueue(new Event((int)EventType.Error,
					new ErrorEvent(Error.CantSendKeepalive), this_));
			}
		}

		/// <summary>
		/// Handles lockout timeout. Start CallManager disconnect procedures.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleLockoutTimeoutLockout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Keepalive timer is probably running, so let's stop it.
			this_.keepaliveTimer.Stop();

			followupEvents.Enqueue(new Event((int)EventType.DisconnectRequest, this_));
		}

		/// <summary>
		/// Goes back to the connected state, requeues the KeepaliveAck, and
		/// everything should be back to normal. KeepaliveAck was received
		/// while in lockout.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleLockoutKeepaliveAck(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Stop the lockout timer.
			this_.miscTimer.Stop();

			// We want the KeepaliveAck timer to still be running.
			followupEvents.Enqueue(new Event((int)EventType.KeepaliveAck, this_));
		}

		/// <summary>
		/// Notifies the application that registration is complete--the full
		/// registration beyond just the RegisterAck.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegistered(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Package the device data to send to the application.
			GapiStatusDataInfoType data = this_.device.Registrar.GetDeviceData();
			data.Version = this_.device.Version;

			this_.DeviceStatus(Device.Status.CallManagerRegisterComplete,
				data);
		}

		/// <summary>
		/// Starts a timer to retry a rejected register-token request.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for RegisterTokenReject.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTokenReject(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			RegisterTokenReject message =
				event_.EventMessage as RegisterTokenReject;

			// Convert seconds to milliseconds.
			int wait = (int)message.waitTimeBeforeNextReg * 1000;

			// Make sure the wait time is at least the minimum.
			if (wait < retryTokenRequestMs)
			{
				wait = retryTokenRequestMs;
			}

			this_.miscTimer.Start(wait, this_, (int)TimerType.TokenRetry);

			// Need to cancel the sccprec timer that is waiting for the token
			// stuff to finish.

			// We would need to start the Discovery miscTimer again when this
			// timeout here occurs. Then we would need to track the number of
			// times we get a tokenReject and just go ahead and register. So,
			// let's just keep the Discovery miscTimer going, let it timeout
			// and force us to register.
			// (This line is commented out in the PSCCP code. Don't know whether it is needed.)
			// device.Discoverer.MiscTimer.Stop();

			this_.timeoutsBeforeRegister = -1;
		}

		/// <summary>
		/// Tries another RegisterTokenRequest because the token retry timer
		/// has expired.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTokenRetryTimeout(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			CallManager this_ = stateMachine as CallManager;

			// Only do things for these states.
			if (this_.IsJustConnected)
			{
				followupEvents.Enqueue(new Event((int)EventType.SendToken, this_));
			}
		}

		#region Support methods to the HandleX methods

		/// <summary>
		/// Attempts to connect to the CallManager.
		/// </summary>
		/// <returns>Whether the connect succeeded.</returns>
		private bool Connect()
		{
			LogVerbose("CMg: connecting {0}", connection);

			DeviceStatus(Device.Status.CallManagerOpening);

			// Get a connection to which our device-level handlers are
			// subscribed.
			connection = connectionFactory.GetConnection();

			// Subscribe to all SCCP-message events in order to make them
			// available to the consumer of this class.
			device.SubscribeToAllConnectionEvents(connection);

			bool started = connection.Start();

			return started;
		}

		/// <summary>
		/// Closes connection to the CallManager.
		/// </summary>
		private void Disconnect()
		{
			if (Connected)
			{
				LogVerbose("CMg: disconnecting {0}", connection);

				connection.Shutdown();
				connection = null;	// (This might not be necessary.)

				// Let the application know that we are not connected to a
				// primary CallManager.
				DeviceStatus(Device.Status.CallManagerDown);
			}
			else
			{
				LogVerbose("CMg: {0} not connected; cannot disconnect",
					connection == null ? "?" : connection.ToString());
			}
		}

		/// <summary>
		/// Invokes DeviceStatus in Device using statusData=Misc.
		/// </summary>
		/// <param name="status">Device status.</param>
		/// <param name="data">Data.</param>
		private void DeviceStatus(Device.Status status,
			GapiStatusDataInfoType data)
		{
			device.DeviceStatus(status, Device.StatusData.Misc,
				data);
		}

		/// <summary>
		/// Invokes DeviceStatus in Device using statusData=Misc and
		/// only this CallManager's address as data.
		/// </summary>
		/// <param name="status">Device status.</param>
		private void DeviceStatus(Device.Status status)
		{
			DeviceStatus(status,
				new GapiStatusDataInfoType(callManagerAddress));
		}

		/// <summary>
		/// Starts timer for how long we wait on a KeepaliveAck from the
		/// CallManager.
		/// </summary>
		/// <remarks>
		/// For next keepalive transmission, delay average of what
		/// GetKeepaliveTimeout() returns rather than exactly what it returns.
		/// This avoids having keepalive timers for all devices expiring at the
		/// same time after transitioning from heavy load--they could have all
		/// been queued up in the EventQueue and processed en masse when the
		/// load decreased sufficiently.
		/// </remarks>
		private void StartKeepaliveTimer()
		{
			// Save locally in case changes while we're using it.
			int localKeepaliveTimeoutMs = GetKeepaliveTimeout();
			int nextKeepaliveTimeoutMs =
				rand.Next(localKeepaliveTimeoutMs * 2 * keepaliveJitterPercent / 100) +
				localKeepaliveTimeoutMs - localKeepaliveTimeoutMs * keepaliveJitterPercent / 100;

			if (nextKeepaliveTimeoutMs > 0)
			{
				keepaliveTimer.Start(nextKeepaliveTimeoutMs, this,
					(int)TimerType.WaitingForKeepaliveAck);
			}
		}

		/// <summary>
		/// Determines, based on the current state, how long to wait for a
		/// KeepaliveAck from the CallManager in response to a Keepalive
		/// message.
		/// </summary>
		/// <returns>Time to wait for KeepaliveAck in milliseconds.</returns>
		private int GetKeepaliveTimeout()
		{
			int timeout;

			if (IsState(waitingForConnectResponse) ||
				IsState(connected) ||
				IsState(token) ||
				IsState(waitingForRegisterResponse))
			{
				// Secondary connections use the secondary timeout.
				timeout = device.Discoverer.SecondaryKeepaliveTimeout;
			}
			else if (IsState(registered) || IsState(lockout))
			{
				// Primary connections use the primary timeout.
				timeout = device.Discoverer.PrimaryKeepaliveTimeout;
			}
			else
			{
				timeout = 0;
			}

			return timeout;
		}

		/// <summary>
		/// Starts timer using the miscTimer event-timer object.
		/// </summary>
		private void StartMiscTimer()
		{
			if (IsState(waitingForConnectRetry))
			{
				miscTimer.Start(Discovery.NakToSynRetryMs,
					this, (int)TimerType.WaitingForConnectRetry);
			}
			else if (IsState(waitingForRegisterResponse))
			{
				miscTimer.Start(Discovery.WaitingForRegisterMs *
					Discovery.AckRetriesBeforeLockout,
					this, (int)TimerType.WaitingForRegisterResponse);
			}
			else if (IsState(waitingForUnregisterResponse))
			{
				miscTimer.Start(Discovery.WaitingForUnregisterMs,
					this, (int)TimerType.WaitingForUnregisterResponse);
			}
			else if (IsState(lockout))
			{
				miscTimer.Start(Discovery.LockoutMs,
					this, (int)TimerType.Lockout);
			}
			else
			{
				// Do nothing.

				// TBD - Can this ever happen? Should I have a Debug.Assert()
				// or at least an Error log here?
			}
		}

		/// <summary>
		/// Resets this CallManager abstraction.
		/// </summary>
		private void Reset()
		{
			miscTimer.Stop();
			keepaliveTimer.Stop();

			connectionRetries = 0;
			ackRspRetries = 0;

			// (-1 apparently means that no timeouts are being applied.)
			timeoutsBeforeRegister = -1;

			shutdown = false;
			keepaliveCount = 0;
		}

		/// <summary>
		/// Checks if the CallManager is stuck. Send an alarm for every six
		/// Keepalives that the CallManager does not respond to while waiting
		/// for a response to a registration request.
		/// </summary>
		private void CheckStuck()
		{
			if (IsState(waitingForRegisterResponse))
			{
				if (++keepaliveCount >= stuckAlarmCount)
				{
					Send(new Alarm(Alarm.Severity.Informational,
						device.Discoverer.CloseCauseToString(),
						Alarm.Param1Const.Composite, 0));
					keepaliveCount = 0;
				}
			}
		}

		/// <summary>
		/// Closes while waiting for a response from the CallManager to our
		/// previous connect request.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void CloseWhileWaitingForConnectResponse(ref Queue followupEvents)
		{
			Disconnect();

			Reset();

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, device.Discoverer));
		}

		/// <summary>
		/// Closes CallManager while waiting for response to previous Unregister
		/// message.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void CloseWhileWaitingForUnregisterResponse(ref Queue followupEvents)
		{
			Fallback = false;

			// Make sure the sccpreg sem is cleaned up.
			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterAck,
				new UnregisterAck(UnregisterAck.Status.Ok), device.Registrar));

			// Try to disconnect.
			Disconnect();

			Reset();

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify,
				device.Discoverer));
		}

		/// <summary>
		/// Closes CallManager while registered.
		/// </summary>
		/// <param name="error">Error code.</param>
		/// <param name="followupEvents">Followup events.</param>
		private void CloseWhileRegistered(Error error, ref Queue followupEvents)
		{
			Reset();

			Fallback = false;

			if (error == Error.TcpClose)
			{
				device.Discoverer.AlarmCondition =
					Discovery.Alarm.LastCallManagerResetTcp;
			}

			// Unregister from the CallManager, do not wait for the ACK.
			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterRequest, device.Registrar));

			// Set the shutdown flag so that we issue a disconnectRequest later
			// down the road.
			// (This line is commented out in the PSCCP code. Don't know whether it is needed.)
			//			shutdown = true;

			followupEvents.Enqueue(new Event((int)Registration.EventType.UnregisterAck,
				new UnregisterAck(UnregisterAck.Status.Ok), device.Registrar));

			// Don't really care if we get a response to the unregisterRequest,
			// so let's just issue one now to keep the CallManager state
			// machine happy.
			followupEvents.Enqueue(new Event((int)EventType.UnregisterAck, this));

			followupEvents.Enqueue(new Event((int)EventType.DisconnectRequest, this));

			followupEvents.Enqueue(new Event((int)Discovery.EventType.DeviceNotify, device.Discoverer));
		}

		#region Wrappers for connection access (in case null)

		/// <summary>
		/// Sends an SCCP message to CallManager if there is still a connection
		/// to it.
		/// </summary>
		/// <param name="message">SCCP message to send.</param>
		/// <returns>Whether the message was sent.</returns>
		private bool Send(SccpMessage message)
		{
			bool sent;

			try
			{
				sent = connection.Send(message);
			}
			catch (NullReferenceException)
			{
				log.Write(TraceLevel.Warning,
					"CMg: {0}: attempt to Send {1} on a null connection; ignored",
					this, message);

				// In case connection is null.
				sent = false;
			}

			return sent;
		}

		/// <summary>
		/// Returns IP address of local end of connection to CallManager if
		/// there is still a connection to it.
		/// </summary>
		private IPEndPoint LocalEndPoint
		{
			get
			{
				IPEndPoint endPoint;

				try
				{
					endPoint = connection.LocalEndPoint;
				}
				catch (NullReferenceException)
				{
					log.Write(TraceLevel.Warning,
						"CMg: {0}: attempt to access LocalEndPoint on a null connection; using 0.0.0.0:0",
						this);

					// In case connection is null, return something.
					endPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
				}

				return endPoint;
			}
		}

		/// <summary>
		/// Returns whether we are still connected to a CallManager.
		/// </summary>
		private bool Connected
		{
			get
			{
				bool connected;

				try
				{
					connected = connection.Connected;
				}
				catch (NullReferenceException)
				{
					// In case connection is null.
					connected = false;
				}

				return connected;
			}
		}
		#endregion

		#endregion
		#endregion
		#endregion

		/// <summary>
		/// Translates a timer expiry to a specific event based on the timeout
		/// type.
		/// </summary>
		/// <param name="timer">Timer with associated data hanging off of it.</param>
		public override void TimerExpiry(EventTimer timer)
		{
			LogVerbose("CMg: {0}: timeout expiry: {1}",
				connection, IntToTimerEnumString(timer.TimeoutType));

			switch ((TimerType)timer.TimeoutType)
			{
				case TimerType.WaitingForKeepaliveAck:
					if (IsState(lockout))
					{
						ProcessEvent(new Event((int)EventType.KeepaliveTimeout, this));
					}
					else
					{
						ProcessEvent(new Event((int)EventType.Timeout,
							new TimeoutEvent(timer.TimeoutType), this));
					}
					break;

				case TimerType.WaitingForRegisterResponse:
					ProcessEvent(new Event((int)EventType.WaitingForRegisterResponseTimeout, this));
					break;

				case TimerType.TokenRetry:
					ProcessEvent(new Event((int)EventType.TokenRetryTimeout, this));
					break;

				default:
					ProcessEvent(new Event((int)EventType.Timeout,
						new TimeoutEvent(timer.TimeoutType), this));
					break;
			}
		}

		/// <summary>
		/// Translates an int to a string that represents one of this class'
		/// event types.
		/// </summary>
		/// <param name="enumValue">Int value to translate.</param>
		/// <returns>String that represents the corresponding event-type
		/// enumeration.</returns>
		public override string IntToEventEnumString(int enumValue)
		{
			return Enum.GetName(typeof(EventType), enumValue);
		}

		/// <summary>
		/// Translates an int to a string that represents one of this class'
		/// timer types.
		/// </summary>
		/// <param name="enumValue">Int value to translate.</param>
		/// <returns>String that represents the corresponding timer-type
		/// enumeration.</returns>
		public override string IntToTimerEnumString(int enumValue)
		{
			return Enum.GetName(typeof(TimerType), enumValue);
		}

		#region LogVerbose signatures
		/// <summary>
		/// Logs Verbose diagnostic if logVerbose set to true.
		/// </summary>
		/// <param name="message">String to log.</param>
		private void LogVerbose(string text)
		{
			if (isLogVerbose)
			{
				log.Write(TraceLevel.Verbose, text);
			}
		}

		/// <summary>
		/// Logs Verbose diagnostic if logVerbose set to true.
		/// </summary>
		/// <param name="text">String to log.</param>
		/// <param name="args">Variable number of arguments to apply to format
		/// specifiers in text.</param>
		private void LogVerbose(string text, params object[] args)
		{
			if (isLogVerbose)
			{
				log.Write(TraceLevel.Verbose, text, args);
			}
		}
		#endregion

		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			return (callManagerAddress == null ?
				"CallManager" : callManagerAddress.ToString()) +
				"(" + this.GetHashCode() + ")";
		}

		/// <summary>
		/// Represents a CallManager error event.
		/// </summary>
		internal class ErrorEvent : Message
		{
			/// <summary>
			/// Constructs an ErrorEvent.
			/// </summary>
			/// <param name="error">Type of error that this event represents.</param>
			internal ErrorEvent(Error error)
			{
				this.error = error;
			}

			/// <summary>
			/// Type of error that this event represents.
			/// </summary>
			private Error error;

			/// <summary>
			/// Property whose value is the type of error that this event represents.
			/// </summary>
			internal Error Error { get { return error; } }

			/// <summary>
			/// Returns a string that represents this object.
			/// </summary>
			/// <returns>String that represents this object.</returns>
			public override string ToString()
			{
				return error.ToString();
			}
		}
	}
}
