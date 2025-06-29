using System;
using System.Net;
using System.Collections;
using System.Diagnostics;
using System.Text;
using System.Threading;

using Metreos.Utilities;
using Metreos.Utilities.Selectors;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents the state machine that discovers CallManagers.
	/// </summary>
	/// <remarks>
	/// Was called sccprec, presumably for "recovery," in the PSCCP code.
	/// </remarks>
	[StateMachine(ActionPrefix="Handle", StateDefinitionPrefix="Define")]
	internal class Discovery : SccpStateMachine
	{
		/// <summary>
		/// Constructs a Discovery object.
		/// </summary>
		/// <param name="device">Device on which this object is being
		/// used.</param>
		/// 
		/// <param name="log">Object through which log entries are generated.</param>
		/// <param name="selector">Selector that performs socket I/O for
		/// all connections.</param>
		/// <param name="threadPool">Thread pool to offload processing of
		/// selected actions from selector callback.</param>
		internal Discovery(Device device, LogWriter log,
			SelectorBase selector, Metreos.Utilities.ThreadPool threadPool) :
			base("Dsc", device, log, ref control)
		{
			this.selector = selector;
			this.threadPool = threadPool;

			rand = new Random();

			clientOverrideAddress = null;

			callManagers = new CallManagerCollection(log);
			miscTimer = new EventTimer(log);

			ResetDevice();
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
		/// new Discovery();
		/// </example>
		/// <param name="log">Object through which log entries are generated.</param>
		public Discovery(LogWriter log) : base(log, ref control) { }

		/// <summary>
		/// Object through which StateMachine assures that static state machine
		/// is constructed only once.
		/// </summary>
		private static StateMachineStaticControl control = new StateMachineStaticControl();

		/// <summary>
		/// Generator of random waits before unregistration.
		/// </summary>
		private Random rand;

		/// <summary>
		/// Selector that performs socket I/O for all connections.
		/// </summary>
		private readonly SelectorBase selector;

		/// <summary>
		/// Thread pool to offload processing of selected actions from
		/// selector callback.
		/// </summary>
		private readonly Metreos.Utilities.ThreadPool threadPool;

		/// <summary>
		/// Starts another device with new data, so clear out all existing
		/// data.
		/// </summary>
		/// <remarks>Reset unregisters this device. Consumer must re-register,
		/// if desired.</remarks>
		internal void ResetDevice()
		{
			deviceOpen = false;
			callManagers.Clear();
			deviceType = DeviceType_;
            
			RestartDevice();
		}

		/// <summary>
		/// Restarts get kicked off with the same device data, so we don't reset it.
		/// </summary>
		/// <remarks>Restart unregisters this device and then immediately
		/// re-registers it.</remarks>
		internal void RestartDevice()
		{
			primaryRetries = 0;
			callManagerIndex = -1;	// (Don't know why this is set to -1 instead of 0.)
			startCallManagerIndex = 0;
			callManagerListIterations = 0;
			waitingToFindPrimary = 0;
			primaryKeepaliveTimeout = defaultKeepaliveMs;
			secondaryKeepaliveTimeout = defaultKeepaliveMs;
			registeredWithFallback = false;
			clientResetCause = Device.Cause.Ok;
			reject = false;
		}

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
		/// Fudge factor in milliseconds.
		/// </summary>
		private static volatile int padMs = 3 * 1000;

		/// <summary>
		/// Fudge factor in milliseconds.
		/// </summary>
		internal static int PadMs { set { padMs = value; } }

		/// <summary>
		/// Waiting for Unregister in milliseconds.
		/// </summary>
		private static volatile int waitingForUnregisterMs = 10 * 1000;

		/// <summary>
		/// Waiting for Unregister in milliseconds.
		/// </summary>
		internal static int WaitingForUnregisterMs
		{
			get { return waitingForUnregisterMs; }
			set { waitingForUnregisterMs = value; }
		}

		/// <summary>
		/// Lockout timeout in milliseconds.
		/// </summary>
		private static volatile int lockoutMs = 10 * 60 * 1000;

		/// <summary>
		/// Lockout timeout in milliseconds.
		/// </summary>
		internal static int LockoutMs
		{
			get { return lockoutMs; }
			set { lockoutMs = value; }
		}

		/// <summary>
		/// Waiting for RegisterAck Keepalive in milliseconds.
		/// </summary>
		internal static int WaitingForRegisterMs
		{
			get { return 2 * CallManager.WaitingForRegisterAckKeepaliveMs; }
		}

		/// <summary>
		/// No CallManagers defined in milliseconds.
		/// </summary>
		private static volatile int noCallManagersDefinedMs = 60 * 1000;

		/// <summary>
		/// No CallManagers defined in milliseconds.
		/// </summary>
		internal static int NoCallManagersDefinedMs { set { noCallManagersDefinedMs = value; } }

		/// <summary>
		/// Retry primary in milliseconds.
		/// </summary>
		private static volatile int retryPrimaryMs = 10 * 1000;

		/// <summary>
		/// Retry primary in milliseconds.
		/// </summary>
		internal static int RetryPrimaryMs { set { retryPrimaryMs = value; } }

		/// <summary>
		/// Retries by stack before giving up. App can register again
		/// if it wants to.
		/// </summary>
		private static volatile int maxCallManagerListIterations = 4;

		/// <summary>
		/// Retries by stack before giving up. App can register again
		/// if it wants to.
		/// </summary>
		internal static int MaxCallManagerListIterations { set { maxCallManagerListIterations = value; } }

		/// <summary>
		/// Minimum wait before sending Unregister in seconds.
		/// </summary>
		private static volatile int minWaitForUnregisterSec = (int)(0.5 * 60);

		/// <summary>
		/// Minimum wait before sending Unregister in seconds.
		/// </summary>
		internal static int MinWaitForUnregisterSec { set { minWaitForUnregisterSec = value; } }

		/// <summary>
		/// Maximum wait before sending Unregister in seconds.
		/// </summary>
		private static volatile int maxWaitForUnregisterSec = (int)(5.5 * 60);

		/// <summary>
		/// Maximum wait before sending Unregister in seconds.
		/// </summary>
		internal static int MaxWaitForUnregisterSec { set { maxWaitForUnregisterSec = value; } }

		/// <summary>
		/// Reject in milliseconds.
		/// </summary>
		private static volatile int rejectMs = 60 * 1000;

		/// <summary>
		/// Reject in milliseconds.
		/// </summary>
		internal static int RejectMs { set { rejectMs = value; } }

		private static volatile DeviceType deviceType_ = DeviceType.StationTelecasterMgr;

		internal static DeviceType DeviceType_
		{
			get { return deviceType_; }
			set { deviceType_ = value; }
		}

		private static volatile ProtocolVersion version = ProtocolVersion.Parche;

		internal static ProtocolVersion Version
		{
			get { return version; }
			set { version = value; }
		}

		/// <summary>
		/// Number of Ack retries before lockout.
		/// </summary>
		private static volatile int ackRetriesBeforeLockout = 3;

		/// <summary>
		/// Number of Ack retries before lockout.
		/// </summary>
		internal static int AckRetriesBeforeLockout
		{
			get { return ackRetriesBeforeLockout; }
			set { ackRetriesBeforeLockout = value; }
		}

		/// <summary>
		/// Number of Keepalive timeouts before no TokenRegister.
		/// </summary>
		private static volatile int keepaliveTimeoutsBeforeNoTokenRegister = 3;

		/// <summary>
		/// Number of Keepalive timeouts before no TokenRegister.
		/// </summary>
		internal static int KeepaliveTimeoutsBeforeNoTokenRegister
		{
			get { return keepaliveTimeoutsBeforeNoTokenRegister; }
			set { keepaliveTimeoutsBeforeNoTokenRegister = value; }
		}

		/// <summary>
		/// Number of UnregisterAck retries.
		/// </summary>
		private static volatile int unregisterAckRetries = 1;

		/// <summary>
		/// Number of UnregisterAck retries.
		/// </summary>
		internal static int UnregisterAckRetries
		{
			get { return unregisterAckRetries; }
			set { unregisterAckRetries = value; }
		}

		/// <summary>
		/// Milliseconds to wait in between checking whether this device has
		/// optimized its CallManagers.
		/// </summary>
		private static volatile int devicePollMs = 10 * 1000;

		/// <summary>
		/// Milliseconds to wait in between checking whether this device has
		/// optimized its CallManagers.
		/// </summary>
		internal static int DevicePollMs { set { devicePollMs = value; } }

		/// <summary>
		/// Amount of variance applied to devicePollMs to avoid all timers
		/// synchronizing. Delay still averages out to the value of
		/// devicePollMs.
		/// </summary>
		/// <remarks>
		/// Value is in percent variance, with normal distribution, average
		/// centered on devicePollMs. For example, if set to 10 and
		/// devicePollMs was set to 60, resulting delay would range between 50
		/// and 70.
		/// </remarks>
		private static volatile int devicePollJitterPercent = 50;

		/// <summary>
		/// Amount of variance applied to devicePollMs to avoid all timers
		/// synchronizing. Delay still averages out to the value of
		/// devicePollMs.
		/// </summary>
		internal static int DevicePollJitterPercent { set { devicePollJitterPercent = value; } }

		/// <summary>
		/// Milliseconds to wait after TCP connect failure before we retry the
		/// connect.
		/// </summary>
		private static volatile int nakToSynRetryMs = 250;

		/// <summary>
		/// Milliseconds to wait after TCP connect failure before we retry the
		/// connect.
		/// </summary>
		internal static int NakToSynRetryMs
		{
			get { return nakToSynRetryMs; }
			set { nakToSynRetryMs = value; }
		}

		/// <summary>
		/// Milliseconds to wait for all calls to go idle before checking for
		/// a new primary CallManager again.
		/// </summary>
		private static volatile int callEndMs = 1 * 1000;

		/// <summary>
		/// Milliseconds to wait for all calls to go idle before checking for
		/// a new primary CallManager again.
		/// </summary>
		internal static int CallEndMs { set { callEndMs = value; } }

		/// <summary>
		/// Milliseconds to wait for connections to close before reset/restart.
		/// </summary>
		private static volatile int closeMs = 5 * 1000;

		/// <summary>
		/// Milliseconds to wait for connections to close before reset/restart.
		/// </summary>
		internal static int CloseMs { set { closeMs = value; } }

		/// <summary>
		/// Default milliseconds that the device waits for a KeepaliveAck from
		/// the CallManager in response to a prior Keepalive.
		/// </summary>
		private static volatile int defaultKeepaliveMs = 10 * 1000;

		/// <summary>
		/// Default milliseconds that the device waits for a KeepaliveAck from
		/// the CallManager in response to a prior Keepalive.
		/// </summary>
		internal static int DefaultKeepaliveMs 
		{
			get { return defaultKeepaliveMs; }
			set { defaultKeepaliveMs = value; }
		}

		/// <summary>
		/// Milliseconds to wait after attempting to connect to a CallManager
		/// before making sure we have optimized the CallManagers.
		/// </summary>
		private static volatile int connectMs = 5 * 1000;

		/// <summary>
		/// Milliseconds to wait after attempting to connect to a CallManager
		/// before making sure we have optimized the CallManagers.
		/// </summary>
		internal static int ConnectMs { set { connectMs = value; } }
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
		/// Address provided by consumer that overrides the local address of
		/// the socket connected to the CallManager. Useful if behind firewall.
		/// </summary>
		/// <remarks>No access control needed because only set once when
		/// device is opened.</remarks>
		private IPAddress clientOverrideAddress;
		internal IPAddress ClientOverrideAddress { get { return clientOverrideAddress; } }

		/// <summary>
		/// The list of CallManagers for this client.
		/// </summary>
		/// <remarks>Access control is provided by the underlying class and by
		/// the use of ReaderLock/Unlock().</remarks>
		private readonly CallManagerCollection callManagers;

		/// <summary>
		/// Returns whether we have a CallManager with the specified address.
		/// </summary>
		/// <param name="address">CallManager address to search for.</param>
		/// <param name="matched">Reference to (presumably only) CallManager
		/// with the specified address or null if not found</param>
		/// <returns>Whether we have a CallManager with the specified
		/// address.</returns>
		internal bool HaveCallManager(IPEndPoint address, out CallManager matched)
		{
			matched = callManagers[address];

			return matched != null;
		}

		/// <summary>
		/// Returns whether we have a CallManager with the specified
		/// "high-level" state.
		/// </summary>
		/// <param name="state">CallManager state to search for.</param>
		/// <param name="matched">Reference to first CallManager with the
		/// specified high-level state or null if none found.</param>
		/// <returns>Whether we have a CallManager with the specified
		/// high-level state.</returns>
		internal bool HaveCallManager(CallManager.HighLevelState_ state,
			out CallManager matched)
		{
			matched = callManagers[state];

			LogVerbose("Dsc: {0}: {1}have CallManager in {2} state",
				this, matched == null ? "do not " : "", state);

			return matched != null;
		}

		/// <summary>
		/// Returns whether we have a primary CallManager.
		/// </summary>
		/// <returns>Whether we have a primary CallManager.</returns>
		private bool HavePrimaryCallManager()
		{
			int primaryIndex;	// dummy
			return HavePrimaryCallManager(out primaryIndex);
		}

		/// <summary>
		/// Returns whether we have a primary CallManager and, if we do, also
		/// returns its index in the CallManagers collection.
		/// </summary>
		/// <param name="primaryIndex">Index of primary CallManager or -1 if
		/// none.</param>
		/// <returns>Whether we have a primary CallManager.</returns>
		internal bool HavePrimaryCallManager(out int primaryIndex)
		{
			primaryIndex = -1;

			int i = 0;
			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (callManager.IsPrimary)
					{
						primaryIndex = i;
						break;
					}
					++i;
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			return primaryIndex != -1;
		}

		/// <summary>
		/// Returns whether we have a primary SccpConnection and, if we do,
		/// also returns a reference to the connection.
		/// </summary>
		/// <param name="primaryConnection">Primary SccpConnection or null if
		/// none.</param>
		/// <returns>Whether we have a primary SccpConnection.</returns>
		internal bool HavePrimaryConnection(out SccpConnection primaryConnection)
		{
			primaryConnection = null;

			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (callManager.IsOrWillBePrimary)
					{
						if (!callManager.Shutdown)
						{
							primaryConnection = callManager.Connection;
						}
						break;
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			return primaryConnection != null;
		}

		/// <summary>
		/// Returns whether we have a secondary CallManager and, if we do, also
		/// returns its index in the CallManagers collection.
		/// </summary>
		/// <param name="primaryIndex">Index of secondary CallManager or -1 if
		/// none.</param>
		/// <returns>Whether we have a secondary CallManager.</returns>
		internal bool HaveSecondaryCallManager(out int secondaryIndex)
		{
			secondaryIndex = -1;

			int i = 0;
			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (callManager.IsSecondary)
					{
						secondaryIndex = i;
						break;
					}
					++i;
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			return secondaryIndex != -1;
		}

		/// <summary>
		/// Returns whether we have a secondary CallManager and, if we do, also
		/// returns a reference to that CallManager.
		/// </summary>
		/// <param name="match">Secondary CallManager or null if none.</param>
		/// <returns>Whether we have a secondary CallManager.</returns>
		internal bool HaveSecondaryCallManager(out CallManager match)
		{
			match = null;
			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (callManager.IsSecondary)
					{
						match = callManager;
						break;
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			return match != null;
		}

		/// <summary>
		/// Returns whether we have an "active" CallManager.
		/// </summary>
		/// <returns>Whether we have an "active" CallManager.</returns>
		internal bool HaveActiveCallManager()
		{
			bool have = false;

			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (callManager.IsActive)
					{
						have = true;
						break;
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			return have;
		}

		/// <summary>
		/// Returns whether all CallManagers are idle.
		/// </summary>
		/// <returns>Whether all CallManagers are idle.</returns>
		internal bool AreAllCallManagersIdle()
		{
			bool allIdle = true;
			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (!callManager.IsIdle)
					{
						allIdle = false;
						break;
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			return allIdle;
		}

		/// <summary>
		/// Returns whether all CallManagers are "locked out"--the stack has
		/// given up on all of them.
		/// </summary>
		/// <returns>Whether all CallManagers are "locked out."</returns>
		internal bool AreAllCallManagersLockout()
		{
			bool allLockout = true;
			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (!callManager.IsLockout)
					{
						allLockout = false;
						break;
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			return allLockout;
		}

		/// <summary>
		/// The CallManager currently being processed.
		/// </summary>
		private volatile int callManagerIndex;

		/// <summary>
		/// The actual, current CallManager object based on the
		/// callManagerIndex.
		/// </summary>
		internal CallManager CallManager { get { return callManagers[callManagerIndex]; } }

		/// <summary>
		/// Starting CallManager index.
		/// </summary>
		private volatile int startCallManagerIndex;

		/// <summary>
		/// Counts when we enter retry mode, looking for a primary CallManager.
		/// </summary>
		private volatile int primaryRetries;

		/// <summary>
		/// Iterations over the CallManager list, looking for a primary
		/// CallManager.
		/// </summary>
		private volatile int callManagerListIterations;

		/// <summary>
		/// Timer used for various purposes (we only waiting for one thing at
		/// a time).
		/// </summary>
		/// <remarks>
		/// Access control not needed here because underlying class provides it.
		/// </remarks>
		private readonly EventTimer miscTimer;

		/// <summary>
		/// Counts when we waited for calls to end before finding primary
		/// CallManager.
		/// </summary>
		private volatile int waitingToFindPrimary;

		/// <summary>
		/// Type of SCCP client.
		/// </summary>
		/// <remarks>Access control not needed because this is set when the
		/// consumer opens a device and never modified thereafter.</remarks>
		private DeviceType deviceType;

		/// <summary>
		/// Property whose value is the type of the SCCP client.
		/// </summary>
		internal DeviceType DeviceType { get { return deviceType; } }

		/// <summary>
		/// Milliseconds to wait for KeepaliveAck from primary CallManager.
		/// </summary>
		/// <remarks>
		/// Primary connections use a different timeout than secondary timeouts
		/// (for some unknown reason).
		/// </remarks>
		private volatile int primaryKeepaliveTimeout;

		/// <summary>
		/// Property whose value is milliseconds to wait for KeepaliveAck from
		/// primary CallManager.
		/// </summary>
		internal int PrimaryKeepaliveTimeout
		{
			get { return primaryKeepaliveTimeout; }
			set { primaryKeepaliveTimeout = value == 0 ? defaultKeepaliveMs : value; }
		}

		/// <summary>
		/// Milliseconds to wait for KeepaliveAck from secondary CallManager.
		/// </summary>
		/// <remarks>
		/// Primary connections use a different timeout than secondary timeouts
		/// (for some unknown reason).
		/// </remarks>
		private volatile int secondaryKeepaliveTimeout;

		/// <summary>
		/// Milliseconds to wait for KeepaliveAck from secondary CallManager.
		/// </summary>
		internal int SecondaryKeepaliveTimeout
		{
			get { return secondaryKeepaliveTimeout; }
			set { secondaryKeepaliveTimeout = value == 0 ? defaultKeepaliveMs : value; }
		}

		/// <summary>
		/// Whether device is registered with fallback.
		/// </summary>
		/// <remarks>
		/// If not registered with fallback, device does not pause before
		/// unregistering.
		/// 
		/// Device can only be registered-with-fallback to an SRST-fallback
		/// CallManager.
		/// </remarks>
		private volatile bool registeredWithFallback;

		/// <summary>
		/// Sets whether device is registered with fallback.
		/// </summary>
		/// <param name="registered">Whether device is intended to be
		/// registered with fallback.</param>
		/// <param name="type">CallManager type.</param>
		internal void SetRegisteredWithFallback(bool registered,
			CallManager.CallManagerType type)
		{
			registeredWithFallback =
				registered &&
				type == CallManager.CallManagerType.SrstFallback;
		}

		/// <summary>
		/// Whether the CallManager has rejected the RegisterRequest.
		/// </summary>
		private volatile bool reject;

		/// <summary>
		/// Property whose value is whether the CallManager has rejected the
		/// RegisterRequest.
		/// </summary>
		internal bool Reject { set { reject = value; } }

		/// <summary>
		/// Cause of reset request when initiated by consumer, not CallManager.
		/// </summary>
		/// <remarks>In PSCCCP, reccb->reset_cause</remarks>
		private volatile Device.Cause clientResetCause;

		/// <summary>
		/// Whether the device is open (as opposed to idle).
		/// </summary>
		private volatile bool deviceOpen;

		/// <summary>
		/// Why TCP connection was closed.
		/// </summary>
		internal enum Alarm
		{
			LoadRejected,
			TftpSizeError,
			CompressorError,
			VersionError,
			DiskfullError,
			ChecksumError,
			FileNotFound,
			TftpTimeout,
			TftpAccessError,
			TftpError,
			LastTcpTimeout,
			LastTcpBadAck,
			LastCallManagerResetTcp,
			LastCallManagerAbortTcp,
			LastTcpClosed, /* 14 */
			LastIcmp,
			LastCallManagerNaked,
			LastKeepaliveTimeout,
			LastFailback,
			LastDiag,
			LastKeypad,
			LastReIp,
			LastReset,
			LastRestart,
			LastRegisterReject,
			LastInitialized,
			Last0X2X,
			WaitingForsFroms,
			WaitingForStatedResponseFroms,
			WaitingForsDspAlarms,
			LastPhoneAbort,
			FileAuthFail,
			LastKeypadClose,
			LastKeypadRestart,
			LastKeypadReset,
		}

		/// <summary>
		/// Current alarm condition of the client.
		/// </summary>
		private volatile Alarm alarmCondition;

		/// <summary>
		/// Property whose value is the current alarm condition of the client.
		/// </summary>
		internal Alarm AlarmCondition { set { alarmCondition = value; } }

		/// <summary>
		/// Textual representation of the current alarm condition of the client
		/// that is sent in the Alarm message to the CallManager.
		/// </summary>
		/// <returns>Textual representation of the alarm condition.</returns>
		internal string CloseCauseToString()
		{
			// TBD - I don't know what the format of this string is.
			return "Name=SEP" + device.MacAddress +
				" Load=3.3(2.0) Last=" + alarmCondition.ToString();
		}

		/// <summary>
		/// Type of timer based generally on the context of when the timer is
		/// started.
		/// </summary>
		private enum TimerType
		{
			RetryPrimary,
			DevicePoll,
			WaitingForCallEnd,
			WaitingForRegisterAckKeepalive,
			WaitingForConnect,
			WaitingForRegister,
			WaitingForUnregister,
			WaitingForToken,
			WaitingForRetryTokenRequest,
			RetryUnregisterTimeout,
			WaitingForClose,
			Reject,
		}

		#region Finite State Machine

		#region State declarations
		// (No access control for states because once constructed, they are not changed.)
		private static State idle = null;
		private static State primaryCheck = null;
		private static State findPrimary = null;
		private static State secondaryCheck = null;
		private static State findSecondary = null;
		private static State primaryOptimize = null;
		private static State makePrimaryConnection = null;
		private static State tokenRequest = null;
		private static State primaryUnregisterCallManager = null;
		private static State primaryRegisterCallManager = null;
		private static State secondaryOptimize = null;
		private static State makeSecondaryConnection = null;
		private static State resetting = null;
		#endregion

		/// <summary>
		/// Types of events that can trigger actions within this state machine.
		/// </summary>
		internal enum EventType
		{
			PrimaryCheck,
			FindPrimary,
			Timeout,
			DeviceNotify,
			SecondaryCheck,
			FindSecondary,
			PrimaryOptimize,
			TokenRequest,
			UnregisterRequest,
			RegisterRequest,
			SecondaryOptimize,
			Done,
			Open,
			ConnectAck,
			RegisterAck,
			UnregisterAck,
			Reset,			// Unregistration requested by CallManager, not consumer.
			ResetRequest,	// Unregistration requested by consumer, not CallManager.
		}

		#region Action declarations
		private static ActionDelegate Done = null;
		private static ActionDelegate PrimaryCheck = null;
		private static ActionDelegate PrimaryCheckFindPrimary = null;
		private static ActionDelegate FindPrimaryDeviceNotify = null;
		private static ActionDelegate FindPrimaryTimeout = null;
		private static ActionDelegate SecondaryCheck = null;
		private static ActionDelegate SecondaryCheckFindSecondary = null;
		private static ActionDelegate FindSecondaryDeviceNotify = null;
		private static ActionDelegate FindSecondaryTimeout = null;
		private static ActionDelegate PrimaryOptimize = null;
		private static ActionDelegate PrimaryOptPrimaryOptimize = null;
		private static ActionDelegate MakePrimaryConnectDeviceNotify = null;
		private static ActionDelegate MakePrimaryConnectTimeout = null;
		private static ActionDelegate MakePrimaryConnectTokenRequest = null;
		private static ActionDelegate MakePrimaryConnectUnregisterRequest = null;
		private static ActionDelegate TokenRequestDeviceNotify = null;
		private static ActionDelegate TokenRequestTimeout = null;
		private static ActionDelegate TokenRequestUnregisterRequest = null;
		private static ActionDelegate PrimaryUnregisterCallManagerDeviceNotify = null;
		private static ActionDelegate PrimaryUnregisterCallManagerTimeout = null;
		private static ActionDelegate PrimaryUnregisterCallManagerRegisterRequest = null;
		private static ActionDelegate PrimaryRegisterCallManagerDeviceNotify = null;
		private static ActionDelegate PrimaryRegisterCallManagerTimeout = null;
		private static ActionDelegate SecondaryOptimize = null;
		private static ActionDelegate SecondaryOptSecondaryOptimize = null;
		private static ActionDelegate MakeSecondaryConnectDeviceNotify = null;
		private static ActionDelegate MakeSecondaryConnectTimeout = null;
		private static ActionDelegate Open = null;
		private static ActionDelegate WorkinOnIt = null;
		private static ActionDelegate Reset_ = null;
		private static ActionDelegate ResetRequest = null;
		private static ActionDelegate ResettingTimeout = null;
		private static ActionDelegate ResettingDeviceNotify = null;
		private static ActionDelegate ResettingDone = null;
		#endregion

		#region State-definition methods
		/// <summary>
		/// Defines the idle state where we spend most of our time in between
		/// periodically optimizing our CallManagers.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State(Initial=true)]
		private static void DefineIdle(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				Open);
			state.Add(EventType.PrimaryCheck,		PrimaryCheck,				primaryCheck);
			state.Add(EventType.Timeout,			PrimaryCheck,				primaryCheck);
			state.Add(EventType.DeviceNotify,		PrimaryCheck,				primaryCheck);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done);
		}

		/// <summary>
		/// Defines the primary-check state where we wind up right after being
		/// prodded into checking to make sure we have a primary CallManager
		/// or, if we already have one, a secondary CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefinePrimaryCheck(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.PrimaryCheck);				// Redundant, so ignore.
			state.Add(EventType.FindPrimary,		PrimaryCheckFindPrimary,	findPrimary);
			state.Add(EventType.Timeout,			PrimaryCheck);
			state.Add(EventType.SecondaryCheck,		SecondaryCheck,				secondaryCheck);
			state.Add(EventType.DeviceNotify);				// We're going to restart anyway.
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the find-primary state where are waiting for a CallManager
		/// to respond to our attempt to make it the primary.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineFindPrimary(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.Timeout,			FindPrimaryTimeout);
			state.Add(EventType.DeviceNotify,		FindPrimaryDeviceNotify);
			state.Add(EventType.SecondaryCheck,		SecondaryCheck,				secondaryCheck);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the secondary-check state where we wind up right after
		/// being prodded into checking to make sure we have a secondary
		/// CallManager.
		/// </summary>
		/// <remarks>
		/// A secondary CallManager is apparently just a CallManager with which
		/// we have established a connection but we are not registered.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineSecondaryCheck(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.FindSecondary,		SecondaryCheckFindSecondary,findSecondary);
			state.Add(EventType.PrimaryOptimize,	PrimaryOptimize,			primaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the find-secondary state where are waiting for a CallManager
		/// to respond to our attempt to make it the secondary.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineFindSecondary(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.Timeout,			FindSecondaryTimeout);
			state.Add(EventType.DeviceNotify,		FindSecondaryDeviceNotify);
			state.Add(EventType.PrimaryOptimize,	PrimaryOptimize,			primaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the primary-optimize state where we see if we can migrate to
		/// a higher-priority CallManager (earlier in the CallManager list) or,
		/// if not, see if we can optimize a secondary CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefinePrimaryOptimize(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.PrimaryOptimize,	PrimaryOptPrimaryOptimize,	makePrimaryConnection);
			state.Add(EventType.SecondaryOptimize,	SecondaryOptimize,			secondaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the make-primary-connection state where we have found a
		/// candidate for primary and now attempt to make it primary.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineMakePrimaryConnection(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.Timeout,			MakePrimaryConnectTimeout);
			state.Add(EventType.DeviceNotify,		MakePrimaryConnectDeviceNotify);
			state.Add(EventType.PrimaryOptimize,	PrimaryOptPrimaryOptimize);
			state.Add(EventType.TokenRequest,		MakePrimaryConnectTokenRequest,
																				tokenRequest);
			state.Add(EventType.UnregisterRequest,	MakePrimaryConnectUnregisterRequest,
																				primaryUnregisterCallManager);
			state.Add(EventType.SecondaryOptimize,	SecondaryOptimize,			secondaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the token-request state where we have requested a "token"
		/// from a CallManager which would allow us to register with it.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineTokenRequest(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.Timeout,			TokenRequestTimeout);
			state.Add(EventType.DeviceNotify,		TokenRequestDeviceNotify);
			state.Add(EventType.UnregisterRequest,	TokenRequestUnregisterRequest,
																				primaryUnregisterCallManager);
			state.Add(EventType.SecondaryOptimize,	SecondaryOptimize,			secondaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the primary-unregister-CallManager state where we have
		/// initiated the unregistration from our current primary CallManager
		/// because we have found a higher-priority CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefinePrimaryUnregisterCallManager(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.Timeout,			PrimaryUnregisterCallManagerTimeout);
			state.Add(EventType.DeviceNotify,		PrimaryUnregisterCallManagerDeviceNotify);
			state.Add(EventType.RegisterRequest,	PrimaryUnregisterCallManagerRegisterRequest,
																				primaryRegisterCallManager);
			state.Add(EventType.SecondaryOptimize,	SecondaryOptimize,			secondaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the primary-register-CallManager state where we have
		/// unregistered with our primary and will next register with the
		/// secondary when the timer expires.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefinePrimaryRegisterCallManager(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.Timeout,			PrimaryRegisterCallManagerTimeout);
			state.Add(EventType.DeviceNotify,		PrimaryRegisterCallManagerDeviceNotify);
			state.Add(EventType.PrimaryCheck,		PrimaryCheck,				primaryCheck);
			state.Add(EventType.SecondaryCheck,		SecondaryCheck,				secondaryCheck);
			state.Add(EventType.SecondaryOptimize,	SecondaryOptimize,			secondaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the secondary-optimize state where we make sure that we
		/// have the highest-priority secondary CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineSecondaryOptimize(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.SecondaryOptimize,	SecondaryOptSecondaryOptimize,
																				makeSecondaryConnection);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the make-secondary-connection state where we have found a
		/// candidate for secondary and now attempt to make it secondary.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineMakeSecondaryConnection(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Open,				WorkinOnIt);
			state.Add(EventType.Timeout,			MakeSecondaryConnectTimeout);
			state.Add(EventType.DeviceNotify,		MakeSecondaryConnectDeviceNotify);
			state.Add(EventType.SecondaryOptimize,	SecondaryOptSecondaryOptimize);
			state.Add(EventType.Reset,				Reset_);
			state.Add(EventType.ResetRequest,		ResetRequest,				resetting);
			state.Add(EventType.Done,				Done,						idle);
		}

		/// <summary>
		/// Defines the resetting state where we have initiated the closing of
		/// all CallManagers and are now simply waiting for all of them to
		/// close.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineResetting(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Timeout,			ResettingTimeout);
			state.Add(EventType.DeviceNotify,		ResettingDeviceNotify);
			state.Add(EventType.Done,				ResettingDone,				idle);
		}
		#endregion

		#region State-machine actions and their delegates

		/// <summary>
		/// Cleans up.
		/// </summary>
		/// <remarks>
		/// This method is called as a final catch for the Discovery state
		/// machine after it has finished running though the whole state
		/// machine and during error cases. The method tries to ensure that
		/// a primary is defined and, if not, initiates the proper
		/// procedures to find one.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleDone(StateMachine stateMachine, Event event_,
			ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			if (!this_.callManagers.IsEmpty)
			{
				// Check if all the CallManagers are lockout. If so, let's
				// request a reset from the application so the reset cleans
				// things up by resetting the device and starting over.
				if (this_.AreAllCallManagersLockout())
				{
					this_.ProcessResetRequest(
						Device.Cause.NoCallManagerFound, ref followupEvents);
				}
				else
				{
					// Make sure we have a primary.
					if (!this_.HavePrimaryCallManager())
					{
						// Are we in retry mode? If not, then we can just go
						// ahead and kick of the state machine again to find a
						// primary.
						if (this_.primaryRetries == 0)
						{
							this_.log.Write(TraceLevel.Info,
								"Dsc: {0}: no primary; trying now", this_);

							followupEvents.Enqueue(new Event((int)EventType.PrimaryCheck, this_));
						}
						else
						{
							// Start timer for next polling. We don't have a
							// primary so we wait a little while and start the
							// process over.
							this_.log.Write(TraceLevel.Warning,
								"Dsc: {0}: no primary, in retry mode, retries: {1}, list iterations: {2}",
								this_, this_.primaryRetries,
								this_.callManagerListIterations);

							this_.miscTimer.Start(retryPrimaryMs,
								this_, (int)TimerType.RetryPrimary);

							// Reset the flag so that when we timeout and come
							// back to this method, we enter the if part of
							// this conditional and start looking for the
							// primary again.
							this_.primaryRetries = 0;
						}
					}
					else
					{
						// Clean up any secondaries.
						this_.CleanupSecondaries(ref followupEvents);

						// Everything looks good. Start timer for next polling
						// and wait. This is where we keep starting the state
						// machine to check if we have everything optimized.
						this_.StartMiscTimerForDevicePoll();
					}
				}
			}
		}

		/// <summary>
		/// Ensures that a primary is defined and, if not, initiates the proper
		/// procedures to find one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for TimeoutEvent, among others.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryCheck(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Check if this was a real timeout to add debug. There are other
			// functions which call this method.
			if (event_.Id == (int)EventType.Timeout)
			{
				TimeoutEvent message = event_.EventMessage as TimeoutEvent;

				// Toss extra WaitForClose timeouts. Discovery generates a
				// timeout event for each DeviceNotify it receives when
				// resetting but cleans up after receiving the first timeout
				// so these extra timeouts are still around.
				if (!message.IsEvent((int)TimerType.WaitingForClose))
				{
					this_.InitiateFindPrimary(ref followupEvents);
				}
			}
			else
			{
				this_.miscTimer.Stop();
				this_.InitiateFindPrimary(ref followupEvents);
			}
		}

		/// <summary>
		/// Finds a primary from the CallManager list. First tries to find the
		/// highest priority connected CallManager and registers with it. If
		/// one is not found, tries to find an idle CallManager and connects
		/// with it.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryCheckFindPrimary(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Search for a secondary that is already connected. We use the
			// term secondary, but don't think we are looking for secondary
			// CallManager. We are just trying to find a free CallManager that
			// is connected. Any CallManager that is connected and not
			// registered is a secondary.

			// Start from the highest priority because this is is the first
			// time through the list.
			this_.callManagers.ReaderLock();
			try
			{
				bool waitForNotification = false;
				for (int i = 0; i < this_.callManagers.Count; ++i)
				{
					// Look for connected and token CallManagers.
					if (this_.callManagers[i].IsConnected)
					{
						// Got one that is connected, let's try and register
						// with it.

						// First time with this CallManager, so let's mark it
						// so.
						this_.primaryRetries = 0;

						// Save the working index.
						this_.callManagerIndex = i;

						// Save starting index so we only look through the
						// list one time.
						this_.startCallManagerIndex = i;

						this_.RegisterCallManager(this_.callManagers[i], ref followupEvents);

						// Wait for notification.
						waitForNotification = true;
						break;
					}
				}

				if (!waitForNotification)
				{
					// No secondary. We need to find an idle CallManager, make
					// a connection and then register.
					bool waitForConnectAck = false;
					for (this_.callManagerIndex = 0;
						this_.callManagerIndex < this_.callManagers.Count;
						++this_.callManagerIndex)
					{
						if (this_.callManagers[this_.callManagerIndex].IsIdle)
						{
							this_.primaryRetries = 0;

							// Tell the CallManager to try and connect.
							this_.ConnectCallManager(this_.callManagers[this_.callManagerIndex], ref followupEvents);

							// Save starting index so we only look through the
							// list one time.
							this_.startCallManagerIndex = this_.callManagerIndex;

							// Wait around for the connectAck.
							waitForConnectAck = true;
							break;
						}
					}

					if (!waitForConnectAck)
					{
						// No CallManager found; wait for next time and try
						// again.
						this_.log.Write(TraceLevel.Warning,
							"Dsc: {0}: no CallManager found to make primary",
							this_);

						// Track that we are in retry mode. The flag is be
						// checked after we go through the whole state machine.
						++this_.primaryRetries;

						// Make sure we do not retry too many times.
						if (!this_.CountListIterations(ref followupEvents))
						{
							// Didn't find a primary. Let's just let the
							// cleanup method fix everything.
							followupEvents.Enqueue(new Event((int)EventType.Done, this_));
						}
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Ensures that a secondary is defined and, if not, initiates the proper
		/// procedures to find one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleFindPrimaryDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			if (this_.reject)
			{
				// The CallManager has rejected the register request.
				// Set a timer to retry.
				this_.miscTimer.Start(rejectMs, this_,
					(int)TimerType.Reject);
			}
			else
			{
				// Validate the index.
				CallManager callManager;
				if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
					out callManager))
				{
					this_.log.Write(TraceLevel.Error,
						"Dsc: {0}: invalid CallManager index {1}",
						this_, this_.callManagerIndex);
				}
				else
				{
					// Are we registered yet?
					if (callManager.IsRegistered)
					{
						this_.LogVerbose("Dsc: {0}: registered {1}",
							this_, callManager);

						this_.miscTimer.Stop();

						// Reset the CallManager list iteration count.
						this_.callManagerListIterations = 0;

						// Let the application know that we have a primary.
						this_.OpenDeviceResponse(Device.Cause.Ok);

						// Got a primary, let's look for secondary.
						followupEvents.Enqueue(new Event((int)EventType.SecondaryCheck, this_));
					}
					else
					{
						// Try to register with this CallManager if it is still
						// connected. Don't check the token states.
						if (callManager.IsJustConnected)
						{
							this_.LogVerbose("Dsc: {0}: connected {1}",
								this_, callManager);

							this_.RegisterCallManager(callManager, ref followupEvents);
						}
						else 
						{
							// If we are idle or lockout then get next.
							if (callManager.IsIdle || callManager.IsLockout)
							{
								this_.miscTimer.Stop();

								this_.log.Write(TraceLevel.Warning,
									"Dsc: {0}: connect or register failed for {1}; trying to find next",
									this_, callManager);

								// Try next.

								HandleFindPrimaryTimeout(this_, event_, ref followupEvents);
							}
							else
							{
								// Just wait, another connection must have
								// changed.
							}
						}
					}
				}
			}
		}

		/// <summary>
		/// Ensures that a primary is defined and, if not, initiates the proper
		/// procedures to find one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for TimeoutEvent, among others.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleFindPrimaryTimeout(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Check if this was a real timeout to add debug. There are other
			// functions which call this method.
			if (event_.Id == (int)EventType.Timeout)
			{
				TimeoutEvent message = event_.EventMessage as TimeoutEvent;

				if (message.IsEvent((int)TimerType.Reject))
				{
					this_.ProcessResetRequest(
						Device.Cause.CallManagerRegisterReject, ref followupEvents);
				}
				else
				{
					this_.FindPrimary(ref followupEvents);
				}
			}
			else
			{
				this_.FindPrimary(ref followupEvents);
			}
		}

		/// <summary>
		/// Ensures that a secondary is defined and, if not, initiates the
		/// proper procedures to find one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSecondaryCheck(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Make sure we have a primary. No need to look for a scondary if
			// don't have a primary.
			if (!this_.HavePrimaryCallManager())
			{
				// No primary. Go back and try to find a primary.
				followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));
			}
			else
			{
				// Make sure we can allocate a secondary. Gotta have at least
				// two CallManagers defined--one for the primary, one for
				// secondary.
				if (this_.callManagers.Count < 2)
				{
					// Not enough CallManagers defined to have a secondary.
					// Goto next state.
					this_.LogVerbose("Dsc: {0}: not enough CallManagers to have secondary",
						this_);

					followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));
				}
				else
				{
					// Check if we already have a secondary. The primary is in
					// a registered state so the first other CallManager in a
					// connected state is a secondary.
					bool haveSecondary = false;
					this_.callManagers.ReaderLock();
					try
					{
						foreach (CallManager callManager in this_.callManagers)
						{
							// Include connected and token CallManagers.
							if (callManager.IsConnected)
							{
								// Found a secondary. Goto next state.
								followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));

								haveSecondary = true;
								break;
							}
						}
					}
					finally
					{
						this_.callManagers.ReaderUnlock();
					}

					if (!haveSecondary)
					{
						// Don't have a secondary, let's try to find one.
						this_.LogVerbose(
							"Dsc: {0}: no secondary; finding", this_);

						followupEvents.Enqueue(new Event((int)EventType.FindSecondary, this_));
					}
				}
			}
		}

		/// <summary>
		/// Ensures that a secondary is defined and, if not, initiates the
		/// proper procedures to find one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSecondaryCheckFindSecondary(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Reset the working index.
			this_.callManagerIndex = 0;

			// Look through all the CallManagers to find one that is idle so we
			// can try to connect with it. And of course, start at the top
			// since the list is descending priority.
			this_.callManagers.ReaderLock();
			try
			{
				while (true)
				{
					if (this_.callManagers[this_.callManagerIndex].IsIdle)
					{
						this_.ConnectCallManager(this_.callManagers[this_.callManagerIndex],
							ref followupEvents);

						// Save starting index so that we only look through the
						// list one time.
						this_.startCallManagerIndex = this_.callManagerIndex;

						// Wait for the ack.
						break;
					}

					// No luck, move on to the next.
					this_.GetNextCallManagerIndex();

					// Are we at the end of the list?
					if (this_.callManagerIndex == 0)
					{
						this_.log.Write(TraceLevel.Error,
							"Dsc: {0}: no CallManagers found with which to connect",
							this_);

						// Could not find secondary, wait for timeout.
						this_.StartMiscTimerForDevicePoll();
						break;
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Ensures that a secondary is defined and, if not, initiates the
		/// proper procedures to find one.
		/// </summary>
		/// <remarks>Invoked for TimeoutEvent, among others.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleFindSecondaryTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Make sure we have a primary.
			if (!this_.HavePrimaryCallManager())
			{
				// No primary. Go back and try to find a primary.
				followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));
			}
			else
			{
				// Validate the index.
				CallManager callManager;
				if (this_.callManagers.IsValidIndex(this_.callManagerIndex,
					out callManager))
				{
					// Are we connected yet? Include token.
					if (callManager.IsConnected)
					{
						this_.LogVerbose(
							"Dsc: {0}: connected {1}", this_, callManager);

						this_.miscTimer.Stop();

						// Good to go. Let's make sure we have the optimal
						// primary and secondary.
						followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));
					}
					else
					{
						this_.FindSecondary(ref followupEvents);
					}
				}
				else
				{
					this_.FindSecondary(ref followupEvents);
				}
			}
		}

		/// <summary>
		/// Ensures a secondary is defined and, if not, initiates procedures to
		/// find one. Something has changed in a CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleFindSecondaryDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Validate the index.
			CallManager callManager;
			if (!this_.callManagers.IsValidIndex(this_.callManagerIndex, out callManager))
			{
				this_.miscTimer.Stop();

				this_.log.Write(TraceLevel.Warning,
					"Dsc: {0}: {1} null", this_, this_.callManagerIndex);

				followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));
			}
			else
			{
				// Are we connected yet? If so, then we have our secondary.
				// Include token in the search.
				if (callManager.IsConnected)
				{
					this_.miscTimer.Stop();

					this_.LogVerbose("Dsc: {0}: connected {1}",
						this_, callManager);

					followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));
				}
				else
				{
					// If we are idle or lockout then get next.
					if (callManager.IsIdle || callManager.IsLockout)
					{
						this_.miscTimer.Stop();

						this_.log.Write(TraceLevel.Warning,
							"Dsc: {0}: connect failed for {1}",
							this_, callManager);

						// Try next.
						HandleFindSecondaryTimeout(this_, event_, ref followupEvents);
					}
					else
					{
						// Just wait, another connection must have changed.
					}
				}
			}
		}

		/// <summary>
		/// Ensures that the current primary is the highest priority and, if
		/// not, initiates the proper procedures to find a better one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryOptimize(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Check if we have a primary.
			this_.callManagers.ReaderLock();
			try
			{
				int primaryIndex;
				if (this_.HavePrimaryCallManager(out primaryIndex))
				{
					// Are we already optimized? Remember the CallManager list
					// is in descending order so index 0 is optimal.
					if (primaryIndex == 0)
					{
						this_.LogVerbose(
							"Dsc: {0}: optimizing primary {1}; found",
							this_, this_.callManagers[primaryIndex]);

						// Yes, check for the optimal secondary.
						followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
					}
					else
					{
						// Let's see if we can get a better primary, starting
						// with the optimal choice of 0. We check all
						// CallManagers with a higher priority than our current
						// primary.
						this_.startCallManagerIndex = 0;

						bool allIdle = false;
						for (this_.callManagerIndex = this_.startCallManagerIndex;
							this_.callManagerIndex < primaryIndex &&
							this_.callManagerIndex < this_.callManagers.Count;
							++this_.callManagerIndex)
						{
							// Validate the index.
							CallManager callManager;
							if (this_.callManagers.IsValidIndex(this_.callManagerIndex,
								out callManager))
							{
								// Find an available CallManager. Include token
								// in the search.
								if (callManager.IsIdle ||
									callManager.IsConnected)
								{
									// Make sure we have no active streams on
									// the current primary.
									if (!this_.AnyViableCalls())
									{
										// Let's try and switch.
										this_.log.Write(TraceLevel.Info,
											"Dsc: {0}: no active streams on {1}; trying to switch to {2}",
											this_, this_.callManagers[primaryIndex],
											callManager);

										followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));

										allIdle = true;
										break;
									}
									else
									{
										// Active streams - can't switch.
										// Move to the next state.
										this_.log.Write(TraceLevel.Warning,
											"Dsc: {0}: active streams on {1}; unable to switch to {2}",
											this_, this_.callManagers[primaryIndex],
											callManager);
									}
								}
							}
						}

						if (!allIdle)
						{
							this_.log.Write(TraceLevel.Warning,
								"Dsc: {0}: no connection to a better primary; skipping",
								this_);
							followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
						}
					}
				}
				else
				{
					this_.log.Write(TraceLevel.Error, "Dsc: {0}: no primary; skipping",
						this_);
					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Ensures that the current primary is the highest priority and, if
		/// not, initiates the proper procedures to find a better one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryOptPrimaryOptimize(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Make sure we have a primary.
			if (!this_.HavePrimaryCallManager())
			{
				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
					this_);

				// No primary. Let the secondary optimize code handle
				// finding the primary.
				followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
			}
			else
			{
				// Try to connect to higher priority secondary.

				// Validate the index.
				CallManager callManager;
				if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
					out callManager))
				{
					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: {1} null; skipping",
						this_, callManager);

					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
				else
				{
					// Check if the secondary is already connected. Don't look
					// at those in the token state, since that is the state we
					// enter if we try to switch to this CallManager.
					if (callManager.IsJustConnected)
					{
						// Got a higher priority secondary so we need to
						// fallback to this one. We need to grab a token before
						// we can fallback.
						this_.LogVerbose(
							"Dsc: {0}: {1} connected; requesting token",
							this_, callManager);

						followupEvents.Enqueue(new Event((int)EventType.TokenRequest, this_));
					}
					else
					{
						// If this secondary connection is idle...
						if (callManager.IsIdle)
						{
							// Tell the CallManager to try and connect.
							this_.ConnectCallManager(callManager, ref followupEvents);
						}
						else
						{
							followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
						}
					}
				}
			}
		}

		/// <summary>
		/// Ensures that we have the optimal primary. If not, then it begins
		/// fallback procedures to find the optimal primary.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleMakePrimaryConnectDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			this_.callManagers.ReaderLock();
			try
			{
				// Make sure we have a primary.
				int primaryIndex;
				if (!this_.HavePrimaryCallManager(out primaryIndex))
				{
					this_.miscTimer.Stop();

					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
						this_);

					// No primary. Let the secondary optimize code handle
					// finding the primary.
					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
				else
				{
					// Validate the index.
					CallManager callManager;
					if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
						out callManager))
					{
						this_.miscTimer.Stop();

						this_.log.Write(TraceLevel.Warning, "Dsc: {0}: {1} null; skipping",
							this_, this_.callManagerIndex);

						followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
					}
					else
					{
						// Unregister from the current primary if we get a
						// token from the new higher priority CallManager.
						if (callManager.IsToken)
						{
							this_.miscTimer.Stop();

							this_.log.Write(TraceLevel.Info, "Dsc: {0}: {1} has token",
								this_, callManager);

							followupEvents.Enqueue(new Event((int)EventType.UnregisterRequest, this_));
						}
						else
						{
							// Request a token if we are connected to the
							// higher priority CallManager.
							if (callManager.IsConnected)
							{
								this_.miscTimer.Stop();

								this_.LogVerbose(
									"Dsc: {0}: {1} connected; requesting token",
									this_, callManager);

								followupEvents.Enqueue(new Event((int)EventType.TokenRequest, this_));
							}
							else
							{
								// If we are idle or lockout then get next.
								if (callManager.IsIdle ||
									callManager.IsLockout)
								{
									this_.log.Write(TraceLevel.Warning,
										"Dsc: {0}: {1} idle or lock; skipping",
										this_, callManager);

									this_.GetNextCallManagerIndex();

									// Try to connect to the next best one and
									// not past the current primary.
									bool found = false;
									while (this_.callManagerIndex < primaryIndex &&
										this_.startCallManagerIndex != this_.callManagerIndex)
									{
										// Validate the index.
										// (The PSCCP uses a previously saved
										// "index" here, but we believe that
										// this is a bug and that it should be
										// callManagerIndex.)
										CallManager nextCallManager;
										if (this_.callManagers.IsValidIndex(
											this_.callManagerIndex,
											out nextCallManager))
										{
											// Do we have a token yet? If so,
											// we can unregister the current
											// primary.
											if (nextCallManager.IsToken)
											{
												followupEvents.Enqueue(new Event((int)EventType.UnregisterRequest, this_));

												found = true;
												break;
											}

											// Are we connected yet? If so, we
											// can try to request a token.
											if (nextCallManager.IsConnected)
											{
												followupEvents.Enqueue(new Event((int)EventType.TokenRequest, this_));

												found = true;
												break;
											}

											// Check if we can connect.
											if (nextCallManager.IsIdle)
											{
												this_.log.Write(TraceLevel.Info,
													"Dsc: {0}: optimizing primary {1} to {2}",
													this_,
													this_.callManagers[primaryIndex],
													nextCallManager);

												followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));

												found = true;
												break;
											}
										}

										this_.GetNextCallManagerIndex();
									}

									if (!found)
									{
										// Go to next state, unable to
										// connect and no other choices.
										followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
									}
								}
								else
								{
									// Just wait, another connection must have
									// changed.
								}
							}
						}
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Ensures that we have the optimal primary. If not, then it begins
		/// fallback procedures to find the optimal primary.
		/// </summary>
		/// <remarks>Invoked for TimeoutEvent.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleMakePrimaryConnectTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			this_.callManagers.ReaderLock();
			try
			{
				// Make sure we have a primary.
				int primaryIndex;
				if (!this_.HavePrimaryCallManager(out primaryIndex))
				{
					this_.miscTimer.Stop();

					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
						this_);

					// No primary. Let the secondary optimize code handle
					// finding the primary.
					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
				else
				{
					// Validate the index.
					CallManager callManager;
					if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
						out callManager))
					{
						this_.miscTimer.Stop();

						this_.log.Write(TraceLevel.Warning, "Dsc: {0}: {1} null; skipping",
							this_, this_.callManagerIndex);

						followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
					}
					else
					{
						// We can unregister with the current primary if we
						// got a token from this higher priority CallManager.
						if (callManager.IsToken)
						{
							this_.miscTimer.Stop();

							this_.log.Write(TraceLevel.Info, "Dsc: {0}: {1} has token",
								this_, callManager);

							followupEvents.Enqueue(new Event((int)EventType.UnregisterRequest, this_));
						}
						else
						{
							// We can request a token if we are connected to
							// this higher priority CallManager. Once we get
							// the token we can register with it.
							if (callManager.IsConnected)
							{
								this_.LogVerbose(
									"Dsc: {0}: {1} connected; requesting token",
									this_, callManager);

								followupEvents.Enqueue(new Event((int)EventType.TokenRequest, this_));
							}
							else
							{
								// Or we can just try for another since this
								// one didn't work.
								this_.GetNextCallManagerIndex();

								// Try to connect to the next best one and not
								// past the current primary.
								bool found = false;
								while (this_.callManagerIndex < primaryIndex &&
									this_.startCallManagerIndex != this_.callManagerIndex)
								{
									// Validate the index.
									// (The PSCCP uses a previously saved
									// "index" here, but we believe that this
									// is a bug and that it should be
									// callManagerIndex.)
									CallManager nextCallManager;
									if (this_.callManagers.IsValidIndex(
										this_.callManagerIndex,
										out nextCallManager))
									{
										// Do we have a token yet? If so, we
										// can unregister the current primary.
										if (nextCallManager.IsToken)
										{
											followupEvents.Enqueue(new Event((int)EventType.UnregisterRequest, this_));

											found = true;
											break;
										}

										// Are we connected yet? If so, then we
										// can request a token for this higher
										// priority CallManager.
										if (nextCallManager.IsConnected)
										{
											followupEvents.Enqueue(new Event((int)EventType.TokenRequest, this_));

											found = true;
											break;
										}

										// Check if we can connect.
										if (nextCallManager.IsIdle)
										{
											this_.log.Write(TraceLevel.Info,
												"Dsc: {0}: optimizing primary {1} to {2}",
												this_,
												this_.callManagers[primaryIndex],
												nextCallManager);

											followupEvents.Enqueue(new Event((int)EventType.PrimaryOptimize, this_));

											found = true;
											break;
										}
									}

									this_.GetNextCallManagerIndex();
								}

								if (!found)
								{
									// Goto to next state, unable to connect
									// and no other choices.
									followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
								}
							}
						}
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Requests a token becaue fallback procedures have been initiated.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleMakePrimaryConnectTokenRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Ask the CallManager to grab a token.
			followupEvents.Enqueue(new Event((int)CallManager.EventType.SendToken,
				this_.callManagers[this_.callManagerIndex]));
			
			// Start a timer to supervise the token request. Use the secondary
			// timeout because this is a secondary connection that is
			// requesting the token.
			this_.miscTimer.Start(
				this_.secondaryKeepaliveTimeout *
				keepaliveTimeoutsBeforeNoTokenRegister +
				padMs, this_, (int)TimerType.WaitingForToken);

			// Wait for notification.
		}

		/// <summary>
		/// Begins unregistration procedures.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleMakePrimaryConnectUnregisterRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			this_.callManagers.ReaderLock();
			try
			{
				// Save the connection that we just made.
				this_.startCallManagerIndex = this_.callManagerIndex;

				// Save the current primary and make sure it is still valid.
				int tempIndex;	// Use temp because out can't be volatile.
				if (!this_.HavePrimaryCallManager(out tempIndex))
				{
					this_.log.Write(TraceLevel.Warning,
						"Dsc: {0}: no primary at {1}; skipping",
						this_, this_.callManagerIndex);

					// The secondary optimize finds a way to get a primary.
					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
				else
				{
					this_.callManagerIndex = tempIndex;

					// Use a random wait to help alleviate network overload. It
					// is possible that a link just came up and you don't want
					// all the clients to start banging away on the
					// CallManagers at once.
					int wait = this_.GetWaitBeforeUnregister();

					this_.LogVerbose(
						"Dsc: {0}: waiting {1}ms before unregistering",
						this_, wait);

					if (wait == 0)
					{
						this_.UnregisterCallManager(this_.callManagers[this_.callManagerIndex],
							ref followupEvents);
					}
					else
					{
						// Start a timer to wait.
						this_.miscTimer.Start(wait, this_,
							(int)TimerType.RetryUnregisterTimeout);
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Checks if the notification came from the CallManager requesting the
		/// token. If so, the primary CallManager is unregistered. Otherwise,
		/// waits for the notification from the correct CallManager.
		/// </summary>
		/// <remarks>
		/// This method is called while waiting for a tokenRequest response and
		/// something had happened on a CallManager. 
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Timeout.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTokenRequestDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Make sure we have a primary.
			if (!this_.HavePrimaryCallManager())
			{
				this_.miscTimer.Stop();

				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
					this_);

				// No primary. Let the secondary optimize code handle finding
				// the primary.
				followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
			}
			else
			{
				// Validate the index.
				CallManager callManager;
				if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
					out callManager))
				{
					this_.miscTimer.Stop();

					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: null {1}; skipping",
						this_, this_.callManagerIndex);

					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
				else
				{
					// Unregister the primary CallManager if we have a token
					// from this higher priority CallManager.
					if (callManager.IsToken)
					{
						this_.miscTimer.Stop();

						this_.log.Write(TraceLevel.Info, "Dsc: {0}: {1} has token",
							this_, callManager);

						followupEvents.Enqueue(new Event((int)EventType.UnregisterRequest, this_));
					}
					else
					{
						// Did we lose our connection?
						if (!callManager.IsConnected)
						{
							this_.miscTimer.Stop();

							this_.log.Write(TraceLevel.Error,
								"Dsc: {0}: connection lost to {1}",
								this_, callManager);

							// We lost our connection. Go back to idle state
							// and start over.
							followupEvents.Enqueue(new Event((int)EventType.Done, this_));
						}
						else
						{
							// Just wait, another connection must have changed.
						}
					}
				}
			}
		}

		/// <summary>
		/// Begins unregistration from the current CallManager.
		/// </summary>
		/// <remarks>
		/// This method is called when requesting a token from a higher
		/// priority CallManager fails. 
		/// 
		/// Invoked for TimeoutEvent.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTokenRequestTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Make sure we have a primary.
			if (!this_.HavePrimaryCallManager())
			{
				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
					this_);

				// No primary. Let the secondary optimize code handle finding
				// the primary.
				followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
			}
			else
			{
				// Validate the index.
				CallManager callManager;
				if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
					out callManager))
				{
					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: null {1}; skipping",
						this_, this_.callManagerIndex);

					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
				else
				{
					if (callManager.IsToken)
					{
						this_.log.Write(TraceLevel.Info, "Dsc: {0}: {1} has token",
							this_, callManager);
					}
					else
					{
						this_.log.Write(TraceLevel.Warning,
							"Dsc: {0}: timeout waiting for token from {1}",
							this_, callManager);
					}

					// Whether we have a token or not, we go on and unregister
					// since the CallManager may not support tokens.
					this_.alarmCondition = Alarm.LastFailback;

					followupEvents.Enqueue(new Event((int)EventType.UnregisterRequest, this_));
				}
			}
		}

		/// <summary>
		/// Initiates unregistration.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleTokenRequestUnregisterRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Save the connection that we just made. Remember, if we are here,
			// then we have just connected to a higher priority CallManager.
			// Therefore, we need to unregister from the current primary.
			this_.startCallManagerIndex = this_.callManagerIndex;

			// Make sure we have a primary.
			int tempIndex;	// Use temp because out can't be volatile.
			if (!this_.HavePrimaryCallManager(out tempIndex))
			{
				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
					this_);

				// No primary. Let the secondary optimize code handle finding
				// the primary.
				followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
			}
			else
			{
				this_.callManagerIndex = tempIndex;

				// Validate the index.
				CallManager callManager;
				if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
					out callManager))
				{
					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: null {1}: skipping",
						this_, this_.callManagerIndex);

					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
				else
				{
					this_.UnregisterCallManager(callManager, ref followupEvents);
				}
			}
		}

		/// <summary>
		/// Initiates registration procedures for the current connected
		/// secondary.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryUnregisterCallManagerRegisterRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Restore the connection that we just made. We modified
			// callManagerIndex while unregistering the old primary.
			this_.callManagerIndex = this_.startCallManagerIndex;

			// Validate the index.
			CallManager callManager;
			if (!this_.callManagers.IsValidIndex(this_.callManagerIndex, out callManager))
			{
				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: null {1}; skipping",
					this_, this_.callManagerIndex);

				followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
			}
			else
			{
				// Are we still connected? If so, then try to register.
				if (callManager.IsConnected)
				{
					// CallManager not responding to fallback register
					// requests, so don't try to register on this connected
					// socket. Start a timer, which expires, kills the
					// connection and starts the process over.
					this_.miscTimer.Start(500, this_,
						(int)TimerType.WaitingForRegister);
				}
				else
				{
					this_.log.Write(TraceLevel.Error, "Dsc: {0}: connection lost to {1}",
						this_, callManager);

					followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
				}
			}
		}

		/// <summary>
		/// Ensures that the current secondary is the highest priority and, if
		/// not, initiates the proper procedures to find a better one.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSecondaryOptimize(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			this_.callManagers.ReaderLock();
			try
			{
				// Make sure we have a primary.
				if (!this_.HavePrimaryCallManager())
				{
					this_.log.Write(TraceLevel.Error, "Dsc: {0}: no primary; skipping",
						this_);

					// No primary. The Done event restarts the searching
					// process.
					followupEvents.Enqueue(new Event((int)EventType.Done, this_));
				}
				else
				{
					// Make sure we have a secondary.
					int secondaryIndex;
					if (!this_.HaveSecondaryCallManager(out secondaryIndex))
					{
						this_.LogVerbose(
							"Dsc: {0}: no secondary; skipping", this_);

						followupEvents.Enqueue(new Event((int)EventType.Done, this_));
					}
					else
					{
						// Make sure we can allocate a secondary. Gotta have at
						// least two CallManagers defined--one for the primary,
						// one for secondary.
						if (this_.callManagers.Count < 2)
						{
							// Not enough CallManagers defined to have a
							// secondary. Goto next state.
							this_.LogVerbose(
								"Dsc: {0}: not enough CallManagers to have secondary",
								this_);

							// No biggie; we have a primary. The Done event
							// starts the polling and maybe we find a
							// secondary.
							followupEvents.Enqueue(new Event((int)EventType.Done, this_));
						}
						else
						{
							// Check if our best/optimal secondary is
							// secondary/connected. Remember we want the
							// highest priority CallManagers to be primary and
							// secondary.
							this_.callManagerIndex = 1;
							this_.startCallManagerIndex = 1;

							// Quick check, index 1 would be the optimal
							// secondary. Include token in the search.
							if (this_.callManagers[this_.callManagerIndex].IsConnected)
							{
								this_.LogVerbose(
									"Dsc: {0}: optimizing secondary; found {1}",
									this_, this_.callManagers[this_.callManagerIndex]);

								// We have an optimal/connected secondary. We
								// can finish up and wait for the next poll to
								// restart everything.
								followupEvents.Enqueue(new Event((int)EventType.Done, this_));
							}
							else
							{
								// Check if we can allocate
								// secondary/connection.
								if (this_.callManagers[this_.callManagerIndex].IsIdle)
								{
									this_.LogVerbose(
										"Dsc: {0}: optimizing secondary; trying {1}",
										this_, this_.callManagers[this_.callManagerIndex]);

									followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
								}
								else
								{
									// Start at the top of the list and work
									// our way down to the connected one. This
									// also helps the primary optimize if
									// we connect to the first one.
									this_.startCallManagerIndex = 0;

									bool found = false;
									for (this_.callManagerIndex = this_.startCallManagerIndex;
										this_.callManagerIndex < secondaryIndex &&
										this_.callManagerIndex < this_.callManagers.Count;
										++this_.callManagerIndex)
									{
										// Validate the index.
										CallManager callManager;
										if (this_.callManagers.IsValidIndex(
											this_.callManagerIndex,
											out callManager))
										{
											// Include connected and token.
											if (callManager.IsConnected)
											{
												this_.LogVerbose(
													"Dsc: {0}: optimizing secondary; found {1}",
													this_, callManager);

												followupEvents.Enqueue(new Event((int)EventType.Done, this_));

												found = true;
												break;
											}

											// Check if we can allocate
											// secondary/connection.
											if (callManager.IsIdle)
											{
												this_.log.Write(TraceLevel.Info,
													"Dsc: {0}: optimizing secondary; trying {1}",
													this_, callManager);

												followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));

												found = true;
												break;
											}
										}
									}

									if (!found)
									{
										// There must only be one CallManager
										// defined or none available.
										this_.log.Write(TraceLevel.Info,
											"Dsc: {0}: no better secondary found; skipping",
											this_);

										followupEvents.Enqueue(new Event((int)EventType.Done, this_));
									}
								}
							}
						}
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Opens a new device by verifying the supplied data and starting the
		/// device.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleOpen(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Make sure we are not already open.
			if (this_.deviceOpen)
			{
				// If already open, let consumer know and ignore request.
				this_.OpenDeviceResponse(
					Device.Cause.DeviceAlreadyOpened);
			}
			else
			{
				this_.deviceOpen = true;

				// Initialize Discovery object from data in Open.
				if (this_.OpenInit((OpenDeviceRequest)event_.EventMessage))
				{
					if (this_.callManagers.Count > 0)
					{
						// Got some valid CallManager addresses, so let's look
						// for a primary CallManager.
						followupEvents.Enqueue(new Event((int)EventType.PrimaryCheck, this_));
					}
					else
					{
						// No CallManager addresses, so notify consumer.
						this_.OpenDeviceResponse(
							Device.Cause.CmsUndefined);
						this_.ResetDevice();
					}
				}
				else
				{
					// Bad data in the Open, so notify consumer.
					this_.OpenDeviceResponse(
						Device.Cause.DeviceInvalidData);
					this_.ResetDevice();
				}
			}
		}

		/// <summary>
		/// Generates log entry stating that the stack is attempting to
		/// optimize CallManagers which should result in registering with a
		/// CallManager if we are not already. Therefore, we are already
		/// working on opening a new device, so the Open event is ignored
		/// because it is redundant.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleWorkinOnIt(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			this_.log.Write(TraceLevel.Info, "Dsc: {0}: request to open device is redundant; ignored (Event: {1})",
				this_, event_.EventMessage.ToString());
		}

		/// <summary>
		/// Processes notifications from the CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryUnregisterCallManagerDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents) 
		{
			Discovery this_ = stateMachine as Discovery;

			// Validate the index.
			CallManager callManager;
			if (!this_.callManagers.IsValidIndex(this_.callManagerIndex, out callManager))
			{
				this_.miscTimer.Stop();

				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: null {1}; skipping",
					this_, this_.callManagerIndex);

				followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));    
			}
			else
			{
				// Are we connected? Or in this case unregistered? If so, then
				// we can try to register with the new primary.
				if (callManager.IsConnected)
				{
					this_.miscTimer.Stop();

					this_.log.Write(TraceLevel.Info, "Dsc: {0}: {1} unregistered",
						this_, callManager);

					followupEvents.Enqueue(new Event((int)EventType.RegisterRequest, this_));    
				} 
				else
				{
					// Oh well, we still have to register since our current
					// primary has died.
					if (callManager.IsIdle || callManager.IsLockout)
					{
						this_.miscTimer.Stop();

						this_.log.Write(TraceLevel.Warning,
							"Dsc: {0}: {1} idle or lockout; not unregistered",
							this_, callManager);

						followupEvents.Enqueue(new Event((int)EventType.RegisterRequest, this_));
					}
					else
					{
						// Just wait, another connection must have changed.
					}
				}
			}
		}

		/// <summary>
		/// Processes timeouts while waiting for the primary to unregister.
		/// </summary>
		/// <remarks>Invoked for TimeoutEvent.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryUnregisterCallManagerTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Can't wait all day to unregister. Let's just let the optimize
			// code handle finding a primary.
			followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
		}

		/// <summary>
		/// Processes device notifications from the CallManager while waiting
		/// for a registration response.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryRegisterCallManagerDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Validate the index.
			CallManager callManager;
			if (!this_.callManagers.IsValidIndex(this_.callManagerIndex, out callManager))
			{
				this_.miscTimer.Stop();

				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: null {1}; skipping",
					this_, this_.callManagerIndex);

				followupEvents.Enqueue(new Event((int)EventType.PrimaryCheck, this_));
			}
			else
			{
				// Are we registered? If so, then we can move on.
				if (callManager.IsRegistered)
				{
					this_.miscTimer.Stop();

					this_.LogVerbose("Dsc: {0}: {1} registered",
						this_, callManager);

					// Reset the list counter because we have a priamry.
					this_.callManagerListIterations = 0;

					// Now we need to disconnect the highest connected.
					if (this_.CleanupSecondaries(ref followupEvents))
					{
						followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
					}
					else
					{
						// We need a new secondary.
						this_.log.Write(TraceLevel.Info, "Dsc: {0}: need a new secondary",
							this_);

						followupEvents.Enqueue(new Event((int)EventType.SecondaryCheck, this_));
					}
				}
				else
				{
					// If we are idle or lockout then get next.
					if ((callManager.IsIdle) || callManager.IsLockout)
					{
						this_.miscTimer.Stop();

						this_.log.Write(TraceLevel.Warning,
							"Dsc: {0}: {1} idle or lockout; skipping",
							this_, callManager);

						followupEvents.Enqueue(new Event((int)EventType.PrimaryCheck, this_));
					}
					else
					{
						// Just wait, another connection must have changed.
					}
				}
			}
		}

		/// <summary>
		/// Processes timeouts while waiting for the primary to register.
		/// </summary>
		/// <remarks>Invoked for TimeoutEvent.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandlePrimaryRegisterCallManagerTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Validate the index.
			CallManager callManager;
			if (this_.callManagers.IsValidIndex(this_.callManagerIndex, out callManager))
			{
				// Are we registered?
				if (callManager.IsRegistered)
				{
					this_.miscTimer.Stop();

					this_.LogVerbose("Dsc: {0}: {1} registered",
						this_, callManager);

					this_.callManagerListIterations = 0;

					// Now we need to disconnect the highest connected.
					if (this_.CleanupSecondaries(ref followupEvents))
					{
						followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));
					}
					else
					{
						// We need a new secondary.
						this_.log.Write(TraceLevel.Warning,
							"Dsc: {0}: need a new secondary", this_);

						followupEvents.Enqueue(new Event((int)EventType.SecondaryCheck, this_));
					}

					return;
				}
			}

			// We need a new primary.
			this_.log.Write(TraceLevel.Warning,
				"Dsc: {0}: {1} hasn't completed registration; need new primary",
				this_, callManager);

			// Close the connection for the current CallManager before we start
			// a new one.
			CallManager.ErrorEvent errorEvent = new CallManager.ErrorEvent(
				CallManager.Error.DrPrimaryRegisterTimeout);
			followupEvents.Enqueue(new Event((int)CallManager.EventType.Error,
				errorEvent, callManager));

			// TBD - May need to check for timing problems if the CallManager we
			// disconnect does not go to idle before we start with the
			// primarycheck.
			followupEvents.Enqueue(new Event((int)EventType.PrimaryCheck, this_));
		}

		/// <summary>
		/// Ensures that a primary is available and that a secondary is
		/// available. If not, then the required procedures to find them are
		/// initiated.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSecondaryOptSecondaryOptimize(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			this_.callManagers.ReaderLock();
			try
			{
				// Make sure we have a primary.
				if (!this_.HavePrimaryCallManager())
				{
					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary, skipping",
						this_);

					// No primary. Go back to idle and start over.
					followupEvents.Enqueue(new Event((int)EventType.Done, this_));
				}
				else
				{
					// Check if we have a better secondary. It is possible that
					// we fell back to a higher priority CallManager and the
					// previous primary might be in a connected state. We can
					// make that the highest priority secondary.
					bool found = false;
					for (int i = 0; i < this_.callManagerIndex; ++i)
					{
						// Check if the CallManager is already connected.
						// (The PSCCP uses "reccb->cm_index" here, which is
						// equivalent to our callManagerIndex, but we believe
						// that this is a bug and that it should be "i".)
						if (this_.callManagers[i].IsConnected)
						{
							this_.log.Write(TraceLevel.Info,
								"Dsc: {0}: skipping {1}, {2}: better secondary",
								this_, this_.callManagers[this_.callManagerIndex],
								this_.callManagers[i]);

							followupEvents.Enqueue(new Event((int)EventType.Done, this_));

							found = true;
							break;
						}
					}

					if (!found)
					{
						// Validate the index.
						CallManager callManager;
						if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
							out callManager))
						{
							this_.log.Write(TraceLevel.Warning,
								"Dsc: {0}: null {1}; skipping",
								this_, this_.callManagerIndex);

							followupEvents.Enqueue(new Event((int)EventType.Done, this_));
						}
						else
						{
							// Check if CallManager is secondary/connected.
							this_.ConnectCallManager(callManager, ref followupEvents);
						}
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Processes device notifications from the CallManager. Ensures that a
		/// primary and secondary is available. If not, then the required
		/// procedures to find them are initiated.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleMakeSecondaryConnectDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents) 
		{
			Discovery this_ = stateMachine as Discovery;

			// Make sure we have a primary.
			if (!this_.HavePrimaryCallManager())
			{
				this_.miscTimer.Stop();

				this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
					this_);

				// No primary. Go back to idle and start over.
				followupEvents.Enqueue(new Event((int)EventType.Done, this_));
			}
			else
			{
				// Validate the index.
				CallManager callManager;
				if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
					out callManager))
				{
					this_.miscTimer.Stop();

					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: null {1}; skipping",
						this_, this_.callManagerIndex);

					followupEvents.Enqueue(new Event((int)EventType.Done, this_));
				}
				else
				{
					// Did the CallManager connect? If so we are done.
					if (callManager.IsConnected)
					{
						this_.miscTimer.Stop();

						this_.LogVerbose("Dsc: {0}: {1} connected",
							this_, callManager);

						followupEvents.Enqueue(new Event((int)EventType.Done, this_));
					}
					else
					{
						// Just wait, another connection must have changed.
					}
				}
			}
		}

		/// <summary>
		/// Processes timeouts while waiting for a CallManager to connect.
		/// Ensures that a primary and secondary are available. If not, the
		/// required procedures to find them are initiated.
		/// </summary>
		/// <remarks>Invoked for TimeoutEvent.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleMakeSecondaryConnectTimeout(
			StateMachine stateMachine, Event event_, ref Queue followupEvents) 
		{
			Discovery this_ = stateMachine as Discovery;

			this_.callManagers.ReaderLock();
			try
			{
				// Make sure we have a primary.
				if (!this_.HavePrimaryCallManager())
				{
					this_.miscTimer.Stop();

					this_.log.Write(TraceLevel.Warning, "Dsc: {0}: no primary; skipping",
						this_);

					// No primary. Start over.
					followupEvents.Enqueue(new Event((int)EventType.Done, this_));
				}
				else
				{
					// Make sure we have a secondary.
					int secondaryIndex;
					if (!this_.HaveSecondaryCallManager(out secondaryIndex))
					{
						this_.miscTimer.Stop();

						this_.LogVerbose(
							"Dsc: {0}: no secondary; skipping", this_);

						// No secondary. Start over.
						followupEvents.Enqueue(new Event((int)EventType.Done, this_));
					}
					else
					{
						// Validate the index.
						CallManager callManager;
						if (!this_.callManagers.IsValidIndex(this_.callManagerIndex,
							out callManager))
						{
							this_.miscTimer.Stop();

							this_.log.Write(TraceLevel.Warning,
								"Dsc: {0}: null {1}; skipping",
								this_, this_.callManagerIndex);

							followupEvents.Enqueue(new Event((int)EventType.Done, this_));
						}
						else
						{
							// Are we connected yet? If so, then we are done.
							if (callManager.IsConnected)
							{
								this_.LogVerbose(
									"Dsc: {0}: {1} connected",
									this_, callManager);

								followupEvents.Enqueue(new Event((int)EventType.Done, this_));
							}
							else
							{
								// Let's try another one.
								this_.GetNextCallManagerIndex();

								// Try to connect to the next best one and not
								// past the current secondary.
								bool found = false;
								while (this_.callManagerIndex < secondaryIndex &&
									this_.startCallManagerIndex != this_.callManagerIndex)
								{
									// Validate the index.
									CallManager nextCallManager;
									if (this_.callManagers.IsValidIndex(
										this_.callManagerIndex,
										out nextCallManager))
									{
										// Are we connected yet?
										if (nextCallManager.IsConnected)
										{
											this_.log.Write(TraceLevel.Info,
												"Dsc: {0}: optimizing secondary {1}, to connected {2}",
												this_,
												this_.callManagers[secondaryIndex],
												nextCallManager);

											// Done cleanups any outstanding
											// connections.
											followupEvents.Enqueue(new Event((int)EventType.Done, this_));

											found = true;
											break;
										}

										// Check if we can connect.
										if (nextCallManager.IsIdle)
										{
											this_.log.Write(TraceLevel.Info,
												"Dsc: {0}: optimizing secondary {1}, to try connecting {2}",
												this_,
												this_.callManagers[secondaryIndex],
												nextCallManager);
										
											followupEvents.Enqueue(new Event((int)EventType.SecondaryOptimize, this_));

											found = true;
											break;
										}
									}

									this_.GetNextCallManagerIndex();
								}

								if (!found)
								{
									// Goto to next state, unable to connect
									// and no other choices.
									followupEvents.Enqueue(new Event((int)EventType.Done, this_));
								}
							}
						}
					}
				}
			}
			finally
			{
				this_.callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Processes Reset request message. Notifies the application that it
		/// needs to reset.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Reset.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleReset_(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;
			Reset message = event_.EventMessage as Reset;

			this_.log.Write(TraceLevel.Info, "Dsc: {0}: reset type {1}", this_, message.type);

			// Cancel any timers. There is no need to do the polling
			// looking for primary or secondary CallManagers.
			this_.miscTimer.Stop();

			// Inform the application that we need a reset.
			Device.Cause cause;
			switch (message.type)
			{
				case Reset.ResetType.Reset:

					// Note that releasing all calls here in response to a
					// Reset from the CallManager is not normal behavior for an
					// SCCP device. We are doing it because an administrator
					// would otherwise have no way to clear a stuck--for
					// whatever reason--device. To restore normal behavior,
					// simply comment out this call to ReleaseAllCalls().
					this_.device.ReleaseAllCalls();

					cause = Device.Cause.CallManagerReset;
					break;

				case Reset.ResetType.Restart:
					cause = Device.Cause.CallManagerRestart;
					break;

				default:
					cause = Device.Cause.NoCallManagerFound;
					break;
			}

			// Let the application know that it needs to reset.
			this_.ProcessResetRequest(cause, ref followupEvents);

			// Wait around for the ResetRequest. The application sends a 
			// ResetRequest when it wants the reset to continue.
		}

		/// <summary>
		/// Processes reset request from the application. Sets the alarm
		/// condition and closes all CallManagers. The application should only
		/// invoke this method when all streams are idle.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleResetRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents) 
		{
			Discovery this_ = stateMachine as Discovery;
			CloseDeviceRequest message =
				event_.EventMessage as CloseDeviceRequest;

			// Cancel any timers. There is no need to do the polling
			// to look for CallManagers because we are going to reset.
			this_.miscTimer.Stop();

			this_.clientResetCause = message.cause;

			switch (message.cause)
			{
				case Device.Cause.CallManagerRestart:
					this_.alarmCondition = Alarm.LastRestart;
					break;

				case Device.Cause.CallManagerReset:
					this_.alarmCondition = Alarm.LastReset;
					break;

				default:
					this_.alarmCondition = Alarm.LastKeypad;
					break;
			}

			// Start a timer to wait for the connections to close.
			if (this_.CloseCallManagers(ref followupEvents))
			{
				this_.miscTimer.Start(closeMs, this_,
					(int)TimerType.WaitingForClose);
			}
			else {
				followupEvents.Enqueue(new Event((int)EventType.Timeout,
					new TimeoutEvent((int)TimerType.WaitingForClose), this_));
			}
		}

		/// <summary>
		/// Handles the timeout that occurs while waiting for CallManagers to
		/// close. Discovery closes all CallManagers during a reset and must
		/// wait until all the CallManagers are closed before resetting itself.
		/// </summary>
		/// <remarks>Invoked for TimeoutEvent.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleResettingTimeout(StateMachine stateMachine,
			Event event_, ref Queue followupEvents) 
		{
			Discovery this_ = stateMachine as Discovery;

			followupEvents.Enqueue(new Event((int)EventType.Done, this_));
		}

		/// <summary>
		/// Handles device notifications from the CallManagers. This state is
		/// waiting for the CallManagers to close.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleResettingDeviceNotify(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			// Go ahead and finish the reset if all CallManagers are idle. No
			// need to wait for the timer to pop.
			if (this_.AreAllCallManagersIdle())
			{
				this_.LogVerbose(
					"Dsc: {0}: all CallManagers are idle; finish reset", this_);

				this_.miscTimer.Stop();

				followupEvents.Enqueue(new Event((int)EventType.Done, this_));
			}
		}

		/// <summary>
		/// Goes to the idle state and waits or restarts depending on the
		/// original reset request.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleResettingDone(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Discovery this_ = stateMachine as Discovery;

			switch (this_.clientResetCause)
			{
				case Device.Cause.CallManagerRestart:
					// Goto idle and restart the whole thing...
					this_.RestartDevice();
					followupEvents.Enqueue(new Event((int)EventType.Done, this_));
					break;

				case Device.Cause.CallManagerReset:
				default:
					// Just cleanup - the application will open another
					// device.
					this_.ResetDevice();
					break;
			}

			this_.device.DeviceStatus(Device.Status.ResetComplete);
		}

		#region Support methods to the HandleX methods

		/// <summary>
		/// Start miscTimer as a device-poll timer.
		/// </summary>
		/// <remarks>
		/// For next polling cycle, delay average of devicePollMs rather
		/// than exactly devicePollMs. This avoids having device-poll
		/// timers for all devices expiring at the same time after
		/// transitioning from heavy load--they could have all been queued up
		/// in the EventQueue and processed en masse when the load decreased
		/// sufficiently.
		/// </remarks>
		private void StartMiscTimerForDevicePoll()
		{
			// Save locally in case changes while we're using it.
			int localDevicePollMs = devicePollMs;
			int nextPollingDelayMs =
				rand.Next(localDevicePollMs * 2 * devicePollJitterPercent / 100) +
				localDevicePollMs - localDevicePollMs * devicePollJitterPercent / 100;

			miscTimer.Start(nextPollingDelayMs, this, (int)TimerType.DevicePoll);
		}

		/// <summary>
		/// Calculates a random amount of time to wait before unregistering.
		/// </summary>
		/// <returns>Milliseconds to wait before unregistering.</returns>
		private int GetWaitBeforeUnregister()
		{
			int wait = 0;

			if (!registeredWithFallback)
			{
				wait = rand.Next(minWaitForUnregisterSec * 1000,
					maxWaitForUnregisterSec * 1000);
			}

			return wait;
		}

		/// <summary>
		/// Find an idle CallManager and make it our secondary by establishing
		/// a TCP connection to it.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void FindSecondary(ref Queue followupEvents)
		{
			callManagers.ReaderLock();
			try
			{
				// Let's try another one.
				GetNextCallManagerIndex();

				while (true)
				{
					// Validate the index.
					CallManager callManager;
					if (callManagers.IsValidIndex(callManagerIndex,
						out callManager))
					{
						// Need an idle CallManager so that we can start a
						// connection.
						if (callManager.IsIdle)
						{
							// Tell the CallManager to try and connect.
							ConnectCallManager(callManager, ref followupEvents);

							// Wait around for the connectResponse.
							break;
						}
					}

					// Let's try another one.
					GetNextCallManagerIndex();

					// Have we made a complete pass?
					if (startCallManagerIndex == callManagerIndex)
					{
						// No CallManager found - wait for next time and try
						// again.
						log.Write(TraceLevel.Info,
							"Dsc: {0}: no CallManager found to make secondary",
							this);

						followupEvents.Enqueue(new Event((int)EventType.Done, this));
						break;
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Goes through the CallManager list and closes all but the best
		/// secondary connection to a CallManager. We only want need to
		/// maintain one primary and one secondary.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		/// <returns>Whether any CallManager connections are open after
		/// cleanup.</returns>
		private bool CleanupSecondaries(ref Queue followupEvents)
		{
			ArrayList secondaryCallManagers = new ArrayList();
			int count = 0;

			// TBD - I suppose a CallManager could change state within this
			// lock. IOW, we could have different set of secondaries. Need to
			// address.
			callManagers.ReaderLock();
			try
			{
				// Count total number of secondaries.
				foreach (CallManager callManager in callManagers)
				{
					if (callManager.IsConnected)
					{
						secondaryCallManagers.Add(callManager);
						++count;
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

			// Cleanup extra secondaries starting at the end of the list. If we
			// only have one good secondary, then this loop won't run. The best
			// secondary is near the start of the list.
			for (int i = secondaryCallManagers.Count - 1; i >= 0 && count-- > 1;
				--i)
			{
				log.Write(TraceLevel.Info, "Dsc: {0}: disconnecting secondary {1}",
					this, secondaryCallManagers[i]);

				followupEvents.Enqueue(new Event((int)CallManager.EventType.DisconnectRequest,
					secondaryCallManagers[i] as CallManager));
			}

			return count != 0;
		}

		/// <summary>
		/// Processes a ResetRequest.
		/// </summary>
		/// <remarks>Was SCCP_RESET in PSCCP.</remarks>
		/// <param name="cause">Why we are resetting.</param>
		/// <param name="followupEvents">Followup events.</param>
		private void ProcessResetRequest(Device.Cause cause,
			ref Queue followupEvents)
		{
			device.State = Device.DeviceState.Resetting;

			LogVerbose("Dsc: {0}: cause: {1}, state: {2}",
				this, cause, device.State);

			device.InternalResetCause = cause;

			// Check the reset type which can be either reset or restart. In
			// either case, we should not send the CloseDeviceRequest until we
			// have completed whatever we need to complete.

			// The reset type may also be from a RegisterReject or
			// unableToFindCallManager. In these cases, the application should
			// reply with the CloseDeviceRequest to put the device back to the idle
			// state. The application should then fix the data and restart the
			// device with another OpenDeviceRequest. Or the application can just
			// change the reset type to GAPI_CAUSE_CM_RESTART and we
			// automatically try to restart the device.

			// Only start the reset if there are no calls. We start the
			// reset after all calls have cleared.
			if (device.Calls.IsEmpty)
			{
				followupEvents.Enqueue(new Event((int)EventType.ResetRequest,
					new CloseDeviceRequest(cause), this));
			}
		}

		/// <summary>
		/// Initiates procedures to find a CallManager that we can use as
		/// primary.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void InitiateFindPrimary(ref Queue followupEvents)
		{
			// Make sure a CallManager is defined.
			if (callManagers.IsEmpty)
			{
				log.Write(TraceLevel.Warning,
					"Dsc: {0}: no CallManagers; cannot find primary",
					this);

				// Do nothing because there are no CallManagers defined.
				followupEvents.Enqueue(new Event((int)EventType.Done, this));
			}
			else
			{
				// Do we have a primary?
				if (!HavePrimaryCallManager())
				{
					// Check if we can start looking for the primary. We can
					// only look if there are not active calls--we don't want
					// to mess up any active calls.
					if (AnyViableCalls())
					{
						// Let the application know that we are not connected
						// to a primary CallManager.
						// (Don't know why data=null, since just prints
						// warning.)
						device.DeviceStatus(
							Device.Status.CallManagerDown);

						// Increment this flag. The flag is used to know that
						// we were waiting for calls to complete and might need
						// to do some cleanup in the else part of this
						// conditional.
						waitingToFindPrimary++;

						// We will try to wait for the call to end before
						// trying again.
						miscTimer.Start(callEndMs, this,
							(int)TimerType.WaitingForCallEnd);
					}
					else
					{
						// We might need to do some cleanup if this flag is
						// set.
						if (waitingToFindPrimary != 0)
						{
							// AnyViableCalls() should have nuked any Calls if
							// it returns false.
							Debug.Assert(device.Calls.IsEmpty,
								"SccpStack: still have Calls");

							waitingToFindPrimary = 0;
						}

						// Don't have a primary so we need to find a primary.
						followupEvents.Enqueue(new Event((int)EventType.FindPrimary, this));
					}
				}
				else
				{
					// Already have a primary so check for a secondary.
					followupEvents.Enqueue(new Event((int)EventType.SecondaryCheck, this));
				}
			}
		}

		/// <summary>
		/// Determines whether there are any viable calls on this device.
		/// </summary>
		/// <remarks>
		/// All calls that are not viable (no connection to a CallManager) are
		/// terminated. This is a workaround because all calls should have been
		/// removed before or as a result of the loss of the connection to the
		/// CallManager.
		/// </remarks>
		/// <returns>Whether there are any viable calls on this device.</returns>
		private bool AnyViableCalls()
		{
			bool viableCall = false;
			
			// If Calls collection is empty, we know that there are no viable
			// Calls. The hard part is if it is not empty.
			if (!device.Calls.IsEmpty)
			{
				// Make separate Queue of abandoned Calls because we can't
				// Remove Calls while we iterate through the collection.
				Queue abandonedCalls = new Queue();
				device.Calls.ReaderLock();
				try
				{
					foreach (Call call in device.Calls)
					{
						if (call.Connected)
						{
							viableCall = true;
						}
						else
						{
							abandonedCalls.Enqueue(call);
						}
					}
				}
				finally
				{
					device.Calls.ReaderUnlock();
				}

				// Now Remove any abandoned Calls.
				foreach (Call call in abandonedCalls)
				{
					log.Write(TraceLevel.Warning,
						"Dsc: {0}: call {1} in state {2} not connected to CallManager; terminated",
						this, call, call.CurrentState);

					// Attempt to remove Call by terminating it nicely.
					device.PostReleaseComplete(
						new ReleaseComplete((uint)call.LineNumber,
						ReleaseComplete.Cause.NotConnected));

					device.Calls.Remove(call);
				}
			}

			return viableCall;
		}

		/// <summary>
		/// Terminates and then Clears all Calls for this device.
		/// </summary>
		private void AbortAllCalls()
		{
			// Tell consumer that Calls are being released then Clear the
			// Calls from the stack. (Use WriterLock because we are going to
			// Clear the collection.)
			bool useCookie;
			LockCookie cookie = device.Calls.WriterLock(out useCookie);
			try
			{
				foreach (Call call in device.Calls)
				{
					// Determine diagnostic text and cause.
					string situationText;
					ReleaseComplete.Cause cause;
					if (call.Connected)
					{
						if (device.State == Device.DeviceState.Registered)
						{
							situationText = "connected and registered";
							cause = ReleaseComplete.Cause.Normal;
						}
						else
						{
							situationText = "connected but not registered";
							cause = ReleaseComplete.Cause.NotRegistered;
						}
					}
					else
					{
						situationText = "not connected";
						cause = ReleaseComplete.Cause.NotConnected;
					}

					log.Write(TraceLevel.Warning,
						"Dsc: {0}: aborting call {1} in state {2}, {3}",
						this, call, call.CurrentState, situationText);
					device.PostReleaseComplete(new ReleaseComplete((uint)call.LineNumber, cause));
				}
				device.Calls.Clear();
			}
			finally
			{
				device.Calls.WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Supervises the CallManager list iterations and requests a reset if
		/// we have exceeded the max number of iterations.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		/// <returns>Whether reset requested.</returns>
		private bool CountListIterations(ref Queue followupEvents)
		{
			bool resetRequested = false;

			if (++callManagerListIterations >=
				maxCallManagerListIterations)
			{
				log.Write(TraceLevel.Error,
					"Dsc: {0}: CallManager list-iteration count reached max ({1})",
					this, maxCallManagerListIterations);

				ProcessResetRequest(
					Device.Cause.NoCallManagerFound, ref followupEvents);

				resetRequested = true;
			}

			return resetRequested;
		}

		/// <summary>
		/// Sends a register event to the requested CallManager.
		/// </summary>
		/// <param name="callManager">CallManager with which to
		/// register.</param>
		/// <param name="followupEvents">Followup events.</param>
		private void RegisterCallManager(CallManager callManager, ref Queue followupEvents)
		{
			LogVerbose("Dsc: {0}: trying to register {1}", this, callManager);

			// (This shouldn't be necessary because all Calls should have been
			// terminated before previous unregistration. This is "just in case.")
			AbortAllCalls();

			// Reset the primary Keepalive timeout. The value is later set to
			// the the value returned in the RegisterAck.
			secondaryKeepaliveTimeout = defaultKeepaliveMs;

			followupEvents.Enqueue(new Event((int)CallManager.EventType.RegisterRequest, callManager));

			// Start a supervision timer.
			miscTimer.Start(
				WaitingForRegisterMs *
				ackRetriesBeforeLockout +
				padMs, this, (int)TimerType.WaitingForRegister);
		}

		/// <summary>
		/// Sends a connect event to the requested CallManager.
		/// </summary>
		/// <param name="callManager">CallManager with which to
		/// connect.</param>
		/// <param name="followupEvents">Followup events.</param>
		private void ConnectCallManager(CallManager callManager, ref Queue followupEvents)
		{
			LogVerbose("Dsc: {0}: trying to connect {1}",
				this, callManager);

			followupEvents.Enqueue(new Event((int)CallManager.EventType.ConnectRequest, callManager));

			// Start a supervision timer.
			miscTimer.Start(connectMs + padMs, this,
				(int)TimerType.WaitingForConnect);
		}

		/// <summary>
		/// Sends an unregister event to the requested CallManager.
		/// </summary>
		/// <param name="callManager">CallManager to unregister from.</param>
		/// <param name="followupEvents">Followup events.</param>
		private void UnregisterCallManager(CallManager callManager, ref Queue followupEvents)
		{
			log.Write(TraceLevel.Info, "Dsc: {0}: trying to unregister {1}",
				this, callManager);

			followupEvents.Enqueue(new Event((int)CallManager.EventType.UnregisterRequest, callManager));

			// Start a supervision timer.
			miscTimer.Start(
				waitingForUnregisterMs *
				unregisterAckRetries +
				padMs, this,
				(int)TimerType.WaitingForUnregister);
		}

		/// <summary>
		/// Closes all CallManagers that aren't already closed.
		/// </summary>
		/// <remarks>Send close events to all CallManagers.</remarks>
		/// <param name="followupEvents">Followup events.</param>
		/// <returns>Whether any were closed.</returns>
		private bool CloseCallManagers(ref Queue followupEvents)
		{
			callManagers.ReaderLock();
			try
			{
				foreach (CallManager callManager in callManagers)
				{
					if (!callManager.IsIdle)
					{
						LogVerbose("Dsc: {0}: trying to close {1}",
							this, callManager);

						followupEvents.Enqueue(new Event((int)CallManager.EventType.Close, callManager));
					}
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}

            // Potential hack note: This was changed to return true as a blind bug fix.
            // The old behavior would only return true if a call manager was actually closed
            //  i.e. A Close event was enqueued.
			return true;
		}

		/// <summary>
		/// Find CallManager we can use as primary.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void FindPrimary(ref Queue followupEvents)
		{
			// Validate the index.
			CallManager callManager;
			if (callManagers.IsValidIndex(callManagerIndex, out callManager))
			{
				// Are we registered yet?
				if (callManager.IsRegistered)
				{
					miscTimer.Stop();

					LogVerbose("Dsc: {0}: {1} registered", this, callManager);

					callManagerListIterations = 0;

					// Found a primary, look for secondary.
					followupEvents.Enqueue(new Event((int)EventType.SecondaryCheck, this));
				}
				else
				{
					LookForNextAvailableCallManager(ref followupEvents);
				}
			}
			else
			{
				LookForNextAvailableCallManager(ref followupEvents);
			}
		}

		/// <summary>
		/// Looks for next-available CallManager as part of the
		/// FindPrimaryTimeout action.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void LookForNextAvailableCallManager(ref Queue followupEvents)
		{
			callManagers.ReaderLock();
			try
			{
				// Let's try another one.
				GetNextCallManagerIndex();

				while (true)
				{
					// Have we made a complete pass?
					if (startCallManagerIndex == callManagerIndex)
					{
						// No CallManager found; wait for next time and try
						// again.
						log.Write(TraceLevel.Error,
							"Dsc: {0}: no CallManager found to make primary while looking for next",
							this);

						++primaryRetries;

						// See if we have gone through the list too many times.
						CountListIterations(ref followupEvents);

						// Let's wait a bit and try again.
						followupEvents.Enqueue(new Event((int)EventType.Done, this));

						break;
					}

					// Validate the index.
					CallManager callManager;
					if (callManagers.IsValidIndex(callManagerIndex,
						out callManager))
					{
						// Are we connected yet? Include token in the search.
						if (callManager.IsConnected)
						{
							// Got one that is connected, let's try and
							// register with it.
							RegisterCallManager(callManager, ref followupEvents);

							// Wait for CallManager to respond.
							break;
						}

						if (callManager.IsIdle)
						{
							// Tell the CallManager to try and connect.
							ConnectCallManager(callManager, ref followupEvents);

							// Wait around for the connectResponse.
							break;
						}
					}

					// Let's try another one.
					GetNextCallManagerIndex();
				}
			}
			finally
			{
				callManagers.ReaderUnlock();
			}
		}

		/// <summary>
		/// Increments to next CallManager in list. Wrap if necessary.
		/// </summary>
		private void GetNextCallManagerIndex()
		{
			if (++callManagerIndex >= callManagers.Count)
			{
				callManagerIndex = 0;
			}
		}

		/// <summary>
		/// Response to OpenDeviceRequest according to specified case code.
		/// </summary>
		/// <param name="cause">Indicator for how to respond.</param>
		private void OpenDeviceResponse(Device.Cause cause)
		{
			if (cause == Device.Cause.Ok)
			{
				device.State = Device.DeviceState.Opened;
			}
			else
			{
				device.State = Device.DeviceState.Idle;
				device.PostUnregistered(new Unregistered());
			}

			LogVerbose("Dsc: {0}: cause: {1}, DeviceState: {2}",
				this, cause, device.State);
		}

		/// <summary>
		/// Initializes instance variables with the received data from an open
		/// request. This data needs to persist because we reuse the data while
		/// running through the state machine.
		/// </summary>
		/// <param name="message">OpenDeviceRequest.</param>
		/// <returns>Whether there was a problem with data in the
		/// OpenEventMessage.</returns>
		private bool OpenInit(OpenDeviceRequest message)
		{
			bool ok;

			// Verify that consumer has supplied valid data.
			if (message.macAddress.Length == 0)
			{
				ok = false;
			}
			else
			{
				if (message.macAddress != null)
				{
					device.MacAddress = message.macAddress;
				}
				deviceType = message.deviceType;

				// Set until overriden by RegisterAck?
				device.Version = message.version;

				if (message.clientAddress != null)
				{
					clientOverrideAddress = message.clientAddress;
				}

				// Instantiate CallManager objects from list of CallManager
				// addresses.
				foreach (IPEndPoint address in message.callManagerAddresses)
				{
					// Make sure we have a good address.
					if (address.Address != IPAddress.Parse("0.0.0.0") &&
						address.Port != 0)
					{
						// Create a new CallManager to handle the address.
						callManagers.Add(new CallManager(
							CallManager.CallManagerType.Standard,
							address, device, log, selector, threadPool));
					}
				}

				// Default to the TFTP server as the CallManager if we did not
				// find any valid addresses.
				if (callManagers.IsEmpty &&
					message.tftpAddress.Address != IPAddress.Parse("0.0.0.0") &&
					message.tftpAddress.Port != 0)
				{
					// Create a new CallManager to handle the address.
					callManagers.Add(new CallManager(
						CallManager.CallManagerType.Standard,
						message.tftpAddress, device, log, selector, threadPool));
				}

				// Add SRST as fallback CallManager if necessary.
				// Instantiate CallManager objects from list of SRST addresses.
				foreach (IPEndPoint address in message.srstAddresses)
				{
					// Make sure we have a good address.
					if (address.Address != IPAddress.Parse("0.0.0.0") &&
						address.Port != 0)
					{
						// Only add this SRST CallManager to the list if it
						// isn't already there
						bool found = false;
						callManagers.ReaderLock();
						try
						{
							foreach (CallManager callManager in callManagers)
							{
								if (callManager.IsAddress(address))
								{
									found = true;
									break;
								}
							}
						}
						finally
						{
							callManagers.ReaderUnlock();
						}

						// Add this SRST CallManager to the list if we did not
						// find it as a Standard or TFTP server.
						if (!found)
						{
							// Create a new CallManager to handle the address.
							callManagers.Add(new CallManager(
								CallManager.CallManagerType.SrstFallback,
								address, device, log, selector, threadPool));
						}
					}
				}

				ok = true;
			}

			return ok;
		}

		#endregion
		#endregion
		#endregion

		/// <summary>
		/// Translates a timer expiry to a specific event based on the timeout
		/// type.
		/// </summary>
		/// <param name="timer">Timer with associated data hanging off of
		/// it.</param>
		public override void TimerExpiry(EventTimer timer)
		{
			LogVerbose("Dsc: {0}: timeout expiry: {1}",
				this, IntToTimerEnumString(timer.TimeoutType));

			ProcessEvent(new Event((int)EventType.Timeout,
				new TimeoutEvent(timer.TimeoutType), this));
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
			return device == null ? "Discovery" : device.ToString();
		}
	}
}
