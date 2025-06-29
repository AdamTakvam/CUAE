using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using Metreos.Utilities;
using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents an abstract SCCP call. Two concrete classes,
	/// IncomingCall and OutgoingCall, inherit from this class.
	/// </summary>
	/// <remarks>
	/// Translates, via a state machine, between low-level, SCCP messages and
	/// higher-level, internal Q.931-like call-oriented messages such as Setup
	/// and Alerting. This presents a simplified call model to the consumer.
	/// 
	/// Each call has a call id, unique across all calls for this device, or
	/// device. The call id is not necessarilly unique across devices within
	/// the stack, although, since the starting value is pseudo-random, the
	/// likelihood of two calls on different devices having the same call id
	/// is extremely remote.
	/// 
	/// Was called sccprcc, presumably for "call control," in the PSCCP code.
	/// </remarks>
	[StateMachine(ActionPrefix="Handle", StateDefinitionPrefix="Define")]
	internal class Call : SccpStateMachine
	{
		/// <summary>
		/// Constructs a Call.
		/// </summary>
		/// <param name="device">Device on which this object is being used.</param>
		/// <param name="log">Object through which log entries are generated.</param>
		/// <param name="lineNumber">Line number starting at 1.</param>
		/// <param name="id">Call identifier assigned by CallManager.</param>
		/// <param name="waiting">Type of client messages stack is waiting to send to
		/// consumer when this call is constructed.</param>
		internal Call(Device device, LogWriter log,
			int lineNumber, int id, WaitingToSend waiting) :
			base("Cal", device, log, ref control)
		{
			this.lineNumber = lineNumber;
			this.id = id;
			this.waiting = waiting;

			precedence = new PrecedenceStruct();

			callStartTimeNs = Const.HPTimerNotSet;	// Indicate not set.
			callAnswerTimeNs = Const.HPTimerNotSet;	// Indicate not set.

			// Add this call to the call collection. The consumer that
			// constructs this call can subsequently check whether it was
			// added via the Added property.
			Call otherCall;
			added = device.Calls.Add(this, out otherCall);
			if (added)
			{
				Debug.Assert(otherCall == null,
					"SccpStack: otherCall yet new Call added");

				LogVerbose("Cal: {0}: {1} call constructed", this,
					waiting == WaitingToSend.Setup ? "incoming" : "outgoing");
			}
			else
			{
				// Collision with existing Call. If not Connected, terminate
				// the existing call and allow this new one to take its place
				// with second attempt at adding it.

				if (!otherCall.Connected)
				{
					device.PostReleaseComplete(
						new ReleaseComplete((uint)otherCall.lineNumber,
						ReleaseComplete.Cause.NotConnected));

					device.Calls.Remove(otherCall);

					added = device.Calls.Add(this, out otherCall);
					if (added)
					{
						Debug.Assert(otherCall == null,
							"SccpStack: otherCall yet new Call added during second attempt");

						log.Write(TraceLevel.Warning,
							"Cal: {0}: {1} call constructed after collision with {2} in {3} state; old call terminated",
							this, waiting == WaitingToSend.Setup ? "incoming" : "outgoing",
							otherCall, otherCall.CurrentState);
					}
					else
					{
						log.Write(TraceLevel.Error,
							"Cal: {0}: while attempting to construct {1} call, collision with {2} in {3} state ; ignored",
							this, waiting == WaitingToSend.Setup ? "incoming" : "outgoing",
							otherCall, otherCall.CurrentState);
					}
				}
				else
				{
					log.Write(TraceLevel.Error,
						"Cal: {0}: while attempting to construct {1} call, collision with still-connected {2} in {3} state ; ignored",
						this, waiting == WaitingToSend.Setup ? "incoming" : "outgoing",
						otherCall, otherCall.CurrentState);
				}
			}
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
		/// new Call();
		/// </example>
		/// <param name="log">Object through which log entries are generated.</param>
		public Call(LogWriter log) : base(log, ref control) { }

		/// <summary>
		/// Object through which StateMachine assures that static state machine
		/// is constructed only once.
		/// </summary>
		private static StateMachineStaticControl control = new StateMachineStaticControl();

		private abstract class Const
		{
			public const long HPTimerNotSet = 0;
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
		/// Time when call starts. Just used for logging.
		/// </summary>
		private long callStartTimeNs;

		/// <summary>
		/// Time when call is answered. Just used for logging.
		/// </summary>
		private long callAnswerTimeNs;

		/// <summary>
		/// Whether this call was added to the call collection during
		/// construction.
		/// </summary>
		private readonly bool added;

		/// <summary>
		/// Whether this call was added to the call collection during
		/// construction.
		/// </summary>
		internal bool Added { get { return added; } }

		/// <summary>
		/// Called-party digits for outgoing call. Held here until we can send
		/// KeypadButtons to CallManager.
		/// </summary>
		/// <remarks>Access control not needed. Set to null while not needed.</remarks>
		private string digits;

		/// <summary>
		/// Precedence. Held here from CallState until StartTransmit can be
		/// sent up to the consumer.
		/// </summary>
		/// <remarks>Access control not needed.</remarks>
		private PrecedenceStruct precedence;

		/// <summary>
		/// Connection to CallManager.
		/// </summary>
		private SccpConnection connection;

		/// <summary>
		/// Type of client messages stack is waiting to send to
		/// consumer when this call is constructed.
		/// </summary>
		private volatile WaitingToSend waiting;

		/// <summary>
		/// After the call is created, what are we waiting to send?
		/// </summary>
		internal enum WaitingToSend
		{
			Nothing,
			Setup,
			Proceeding,
			Alerting,
		}

		/// <summary>
		/// Call identifier from the CallManager.
		/// </summary>
		/// <remarks>For an outgoing call, the stack assigns a provisional
		/// value to id until an "official" value arrives from the CallManager.
		/// In incoming call always has the official value from the
		/// CallManager. (In PSCCP, cccb->id.)</remarks>
		private int id;

		/// <summary>
		/// Call identifier from the CallManager.
		/// </summary>
		internal int Id { get { return id; } }

		/// <summary>
		/// Unique conference identifier.
		/// </summary>
		/// <remarks>Access control not needed.</remarks>
		private volatile uint conferenceId;

		/// <summary>
		/// Unique conference identifier.
		/// </summary>
		internal uint ConferenceId { get { return conferenceId; } set { conferenceId = value; } }

		/// <summary>
		/// This is a transaction identifer, which is typically used to
		/// associate a response with its request.
		/// </summary>
		private volatile int passthruPartyId;

		/// <summary>
		/// This is a transaction identifer, which is typically used to
		/// associate a response with its request.
		/// </summary>
		internal int PassthruPartyId { get { return passthruPartyId; } set { passthruPartyId = value; } }

		/// <summary>
		/// Line number.
		/// </summary>
		/// <remarks>In PSCCP, sccp_line.</remarks>
		private volatile int lineNumber;

		/// <summary>
		/// Line number.
		/// </summary>
		internal int LineNumber { get { return lineNumber; } }

		/// <summary>
		/// Updates the call id and line number.
		/// </summary>
		/// <param name="id">New call identifier.</param>
		/// <param name="lineNumber">New line number.</param>
		internal void Update(int id, int lineNumber)
		{
			this.id = id;
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// This property has the value of whether the call is "viable"--is not
		/// idle or releasing.
		/// </summary>
		internal bool Viable
		{
			get { return !IsState(idle) &&
					!IsState(outgoingReleasing) && !IsState(incomingReleasing); }
		}

		/// <summary>
		/// This property has the value of whether the call is in the initiated state.
		/// </summary>
		internal bool Initiated { get { return IsState(callInitiated); } }

		#region Finite State Machine

		#region State declarations
		// (No access control for states because once constructed, they are not changed.)
		private static State idle = null;
		private static State callInitiated = null;
		private static State outgoingProceeding = null;
		private static State outgoingAlerting = null;
		private static State incomingAlerting = null;
		private static State incomingConnecting = null;
		private static State connected = null;
		private static State outgoingReleasing = null;
		private static State incomingReleasing = null;
		#endregion

		/// <summary>
		/// Types of events that can trigger actions within this state machine.
		/// </summary>
		internal enum EventType
		{
			None,				// was SCCPCC_E_MIN in PSCCP
			Setup,
			SetupAck,
			Proceeding,
			Alerting,
			Connect,
			ConnectAck,
			Disconnect,
			Release,
			ReleaseComplete,
			Offhook,
			Onhook,
			DigitsOut,
			DigitIn,
			FeatureRequest,
			CallState,
			CallStateOffhook,
			CallStateSetup,
			CallStateProceeding,
			CallStateAlerting,
			CallStateConnected,
			CallStateRelease,
			UpdateUi,
			Media,
			OpenReceiveResponse,
			Timer,
			Cleanup,
			DeviceToUserDataRequest,
			DeviceToUserDataResponse,
			Passthru,
		}

		#region Action declarations
		private static ActionDelegate Setup = null;
		private static ActionDelegate Connect = null;
		private static ActionDelegate Release = null;
		private static ActionDelegate ReleaseInitiated = null;
		private static ActionDelegate Offhook = null;
		private static ActionDelegate DigitIn = null;
		private static ActionDelegate DigitsOut = null;
		private static ActionDelegate UpdateUi = null;
		private static ActionDelegate CallState_ = null;
		private static ActionDelegate SendDigits = null;
		private static ActionDelegate Media = null;
		private static ActionDelegate OpenReceiveResponse = null;
		private static ActionDelegate IdleFeatureRequest = null;
		private static ActionDelegate FeatureRequest_ = null;
		private static ActionDelegate XxxOffhook = null;
		private static ActionDelegate CallStateOffhook = null;
		private static ActionDelegate CallStateSetup = null;
		private static ActionDelegate CallStateProceeding = null;
		private static ActionDelegate CallStateAlerting = null;
		private static ActionDelegate OutgoingAlertingCallStateConnected = null;
		private static ActionDelegate IcCallStateConnected = null;
		private static ActionDelegate IaReleaseComplete = null;
		private static ActionDelegate OrRelease = null;
		private static ActionDelegate CallStateOnhookRelease = null;
		private static ActionDelegate CallStateRelease = null;
		private static ActionDelegate Cleanup = null;
		private static ActionDelegate DeviceToUserDataRequest = null;
		private static ActionDelegate DeviceToUserDataResponse = null;
		#endregion

		#region State-definition methods
		/// <summary>
		/// Defines the idle state where the call has just been
		/// constructed and essentially nothing else has happened yet.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State(Initial=true)]
		private static void DefineIdle(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Setup,				Setup,						callInitiated);
			state.Add(EventType.Offhook,			Offhook,					callInitiated);
			state.Add(EventType.SetupAck);					// Consumer could respond before CallStateSetup received
			state.Add(EventType.Proceeding);				// Consumer could respond before CallStateSetup received
			state.Add(EventType.Alerting);					// Consumer could respond before CallStateSetup received
			state.Add(EventType.FeatureRequest,		IdleFeatureRequest,			callInitiated);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook,			callInitiated);
			state.Add(EventType.CallStateSetup,		CallStateSetup,				incomingAlerting);
			state.Add(EventType.CallStateRelease);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the call-initiated state where the call has just been
		/// initiated but has not proceeded further.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineCallInitiated(State state)	// Potential outgoing call
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			ReleaseInitiated,			outgoingReleasing);
			state.Add(EventType.ReleaseComplete,	Cleanup,					idle);
			state.Add(EventType.Onhook,				ReleaseInitiated,			outgoingReleasing);
			state.Add(EventType.DigitsOut,			DigitsOut);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	SendDigits);
			state.Add(EventType.CallStateProceeding,CallStateProceeding,		outgoingProceeding);
			state.Add(EventType.CallStateAlerting,	CallStateAlerting,			outgoingAlerting);
			state.Add(EventType.CallStateConnected,	OutgoingAlertingCallStateConnected,
																				connected);
			state.Add(EventType.CallStateRelease,	CallStateOnhookRelease);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the outgoing-proceeding state where the outgoing call has
		/// received a CallState(Proceeding) SCCP message.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineOutgoingProceeding(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			OrRelease,					idle);
			state.Add(EventType.Onhook,				OrRelease,					idle);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook);
			state.Add(EventType.CallStateAlerting,	CallStateAlerting,			outgoingAlerting);
			state.Add(EventType.CallStateConnected,	OutgoingAlertingCallStateConnected,
																				connected);
			state.Add(EventType.CallStateRelease,	CallStateRelease,			incomingReleasing);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the outgoing-alerting state where the outgoing call has
		/// received a CallState(Alerting) SCCP message.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineOutgoingAlerting(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			Release,					outgoingReleasing);
			state.Add(EventType.Onhook,				Release,					outgoingReleasing);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook);
			state.Add(EventType.CallStateConnected,	OutgoingAlertingCallStateConnected,
																				connected);
			state.Add(EventType.CallStateRelease,	CallStateRelease,			incomingReleasing);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the incoming-alerting state where the call has
		/// received a CallState(RingIn) SCCP message, establishing it as an
		/// incoming call.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineIncomingAlerting(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			Release,					outgoingReleasing);
			state.Add(EventType.ReleaseComplete,	IaReleaseComplete);
			state.Add(EventType.Offhook,			Connect,					incomingConnecting);
			state.Add(EventType.Connect,			Connect,					incomingConnecting);
			state.Add(EventType.SetupAck);
			state.Add(EventType.Proceeding);
			state.Add(EventType.Alerting);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook);
			state.Add(EventType.CallStateRelease,	CallStateRelease,			incomingReleasing);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the incoming-connecting state where the consumer has
		/// answered the incoming call.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineIncomingConnecting(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			Release,					outgoingReleasing);
			state.Add(EventType.Onhook,				Release,					outgoingReleasing);
			state.Add(EventType.Offhook,			XxxOffhook);
			state.Add(EventType.DigitIn,			DigitIn);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook);
			state.Add(EventType.CallStateConnected,	IcCallStateConnected,		connected);
			state.Add(EventType.CallStateRelease,	CallStateRelease,			incomingReleasing);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the connected state where the call has received a
		/// CallState(Connected) SCCP message from the CallManager,
		/// acknowledging that the CallManager now considers the call to be
		/// connected, or established.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineConnected(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			Release,					outgoingReleasing);	// We want to release
			state.Add(EventType.Onhook,				Release,					outgoingReleasing);	//		"
			state.Add(EventType.ConnectAck);
			state.Add(EventType.DigitsOut,			DigitsOut);
			state.Add(EventType.DigitIn,			DigitIn);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook);				// TBD - Notify consumer of new incoming call or ignore?
			state.Add(EventType.CallStateProceeding);								// We're already connected, so ignore.
			state.Add(EventType.CallStateAlerting);									//		"
			state.Add(EventType.CallStateConnected);								//		"
			state.Add(EventType.CallStateRelease,	CallStateRelease,			incomingReleasing);	// CallManager wants to release
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the outgoing-releasing state where the consumer has
		/// initiated the termination of the call. The next state is the idle
		/// state. (At this point, incoming/outgoing refers to whether the
		/// CallManager/consumer has initiated the action, not whether this is
		/// an incoming or outgoing call.)
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineOutgoingReleasing(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			OrRelease);
			state.Add(EventType.Onhook,				OrRelease);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook);
			state.Add(EventType.CallStateRelease,	CallStateOnhookRelease);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Defines the incoming-releasing state where the CallManager has
		/// initiated the termination of the call. The next state is the idle
		/// state. (At this point, incoming/outgoing refers to whether the
		/// CallManager/consumer has initiated the action, not whether this is
		/// an incoming or outgoing call.)
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineIncomingReleasing(State state)
		{
			//					Event				Action						Next State
			state.Add(EventType.Release,			Cleanup,					idle);
			state.Add(EventType.Onhook,				Cleanup,					idle);
			state.Add(EventType.ReleaseComplete,	Cleanup,					idle);
			state.Add(EventType.FeatureRequest,		FeatureRequest_);
			state.Add(EventType.CallState,			CallState_);
			state.Add(EventType.CallStateOffhook,	CallStateOffhook);
			state.Add(EventType.CallStateRelease);
			state.Add(EventType.UpdateUi,			UpdateUi);
			state.Add(EventType.Media,				Media);
			state.Add(EventType.OpenReceiveResponse,OpenReceiveResponse);
			state.Add(EventType.Cleanup,			Cleanup,					idle);
			state.Add(EventType.DeviceToUserDataRequest,
													DeviceToUserDataRequest);
			state.Add(EventType.DeviceToUserDataResponse,
													DeviceToUserDataResponse);
		}

		/// <summary>
		/// Cleans up by removing this call from the Calls list.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCleanup(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			// (The two methods need to be atomic with respect to Calls.)
			bool useCookie;
			LockCookie cookie = this_.device.Calls.WriterLock(out useCookie);
			try
			{
				// If consumer sent ReleaseComplete that invokes this method, the
				// call has already been removed so don't try again. It wouldn't
				// hurt but would generate duplicate log entries that might be
				// confusing.
				if (this_.device.Calls.GetCallByCallId(this_.Id) != null)
				{
					this_.device.Calls.Remove(this_);
				}
			}
			finally
			{
				this_.device.Calls.WriterUnlock(cookie, useCookie);
			}
		}

		/// <summary>
		/// Handles offhook event while idle. Send offhook to CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Offhook.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleOffhook(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			OffhookClient message = event_.EventMessage as OffhookClient;

			if (this_.LineNumber != message.Line)
			{
				this_.log.Write(TraceLevel.Warning,
					"Cal: {0}: event {1} changing line number from {2} to {3}",
					this_, event_, this_.LineNumber, message.Line);
			}

			this_.device.Calls.Rekey(this_, this_.Id, (int) message.Line);

			// Wait until we get the CallState message to set the call id.

			// Make sure we are connected to a CallManager.
			if (!this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				this_.SendReleaseCompleteToConsumer(
					ReleaseComplete.Cause.NotConnected, ref followupEvents);

				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
			else
			{
				// (I think call id has to be 0 because CallManager has not yet
				// assigned one to this call.)
				this_.Send(new OffhookSccp((uint)this_.LineNumber, 0));
			}
		}

		/// <summary>
		/// Handles FeatureRequest event. Setup initial cb data and send feature
		/// to CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for FeatureRequest.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleIdleFeatureRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			FeatureRequest message = event_.EventMessage as FeatureRequest;

			if (this_.LineNumber != message.Line)
			{
				this_.log.Write(TraceLevel.Warning,
					"Cal: {0}: event {1} changing line number from {2} to {3}",
					this_, event_, this_.LineNumber, message.Line);
			}

			this_.device.Calls.Rekey(this_, this_.Id, (int) message.Line);

			// Wait until we get the callState message to set the sccp_call_id.
			// (This line is commented out in the PSCCP code. Don't know whether it is needed.)
			//cccb->sccp_call_id = ;

			// Make sure we are connected to a CallManager.
			if (!this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				this_.SendReleaseCompleteToConsumer(
					ReleaseComplete.Cause.NotConnected, ref followupEvents);

				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
			else
			{
				this_.Send(new SoftkeyEvent((uint)message.feature,
					(uint)this_.LineNumber, (uint)this_.Id));

				// (FeatureResponse events are ignored, so don't add the event
				// to the queue.)
			}
		}

		/// <summary>
		/// Handles offhook event during a non-IDLE state. Send offhook to
		/// CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleXxxOffhook(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(new OffhookSccp((uint)this_.LineNumber, (uint)this_.Id));
		}

		/// <summary>
		/// Handles setup event. Setup initial cb data and send offhook to
		/// CallManager. Push digits (if any) back onto the event queue.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Setup.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSetup(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			Setup message = event_.EventMessage as Setup;

			if (this_.LineNumber != message.Line)
			{
				this_.log.Write(TraceLevel.Warning,
					"Cal: {0}: event {1} changing line number from {2} to {3}",
					this_, event_, this_.LineNumber, message.Line);
			}

			this_.device.Calls.Rekey(this_, this_.Id, (int) message.Line);

			// Wait until we get the CallState message to set the call id.

			// Make sure we are connected to a CallManager.
			if (!this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				this_.SendReleaseCompleteToConsumer(
					ReleaseComplete.Cause.NotConnected, ref followupEvents);

				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
			else
			{
				this_.LogPlacingCall(message.callingPartyNumber,
					message.callingPartyName,
					message.calledPartyNumber,
					message.originalCalledPartyNumber);

				// (I think call id has to be 0 because CallManager has not yet
				// assigned one to this call.)
				this_.Send(new OffhookSccp((uint)this_.LineNumber, 0));

				// Copy the digits to temp storage. The digits can not be sent
				// until the CallManager has moved the client to the offhook state.
				if (message.calledPartyNumber.Length > 0)
				{
					this_.digits = message.calledPartyNumber;
				}
			}
		}

		/// <summary>
		/// Handles digits-out event. Send KeypadButton to CallManager.
		/// </summary>
		/// <remarks>Application should not send digits until it receives a
		/// SetupAck.</remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for digits.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleDigitsOut(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			Digits message = event_.EventMessage as Digits;

			foreach (char c in message.digits)
			{
				this_.Send(new KeypadButton(AsciiDigitToButton(c),
					(uint)this_.LineNumber, (uint)this_.Id));
			}
		}

		/// <summary>
		/// Handles digit-in event. We received KeypadButton from CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for digits.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleDigitIn(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			KeypadButton message = event_.EventMessage as KeypadButton;

			this_.device.PostDigit(new Digits((uint)this_.LineNumber,
				ButtonToAsciiDigit(message.button).ToString()));
		}

		/// <summary>
		/// Handles CallState setup event. An incoming call has been received.
		/// Setup object and inform the application of the incoming call.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCallStateOffhook(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			// Make sure we are connected to a primary.
			if (!this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
			else
			{
				// Inform the application of the new incoming call.
				this_.SendOffhookToConsumer(ref followupEvents);
			}
		}

		/// <summary>
		/// Handles CallState setup event. An incoming call has been received.
		/// Setup cb data and inform the application of the incoming call.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCallStateSetup(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			// Make sure we are connected to a primary.
			if (!this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
			else
			{
				// Inform the application of the new incoming call.
				this_.PrepareToSendSetupToConsumer(this_.Id, this_.LineNumber, null, null, ref followupEvents);
			}
		}

		/// <summary>
		/// Handles CallState proceeding event. The CallManager has acknowledged
		/// that it has enough information to proceed with the call. Inform the
		/// application.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCallStateProceeding(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			// Inform the application that CallManager has enough information to
			// proceed with the call.

			// Actually, don't send the message to the consumer yet. We need to
			// wait for the CallInfo message so that we can grab the
			// CalledNumber. The Setup requires a CalledNumber.
		}

		/// <summary>
		/// Handles CallState alerting event. The farend is ringing. Inform the
		/// application.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCallStateAlerting(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			// Inform the application that the farend is ringing.
			this_.device.PostAlerting(new Alerting((uint)this_.LineNumber));
		}

		/// <summary>
		/// Handles CallState connect event during S_OUTGOING_ALERTING. The
		/// farend has answered the call. Send connect to application.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleOutgoingAlertingCallStateConnected(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.LogAnswer();

			// Inform the application that the farend has answered the call.
			this_.device.PostConnect(new Connect((uint)this_.LineNumber));
		}

		/// <summary>
		/// Handles CallState offhook event. The CallManager has awarded the call to
		/// this client. Inform the application.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleIcCallStateConnected(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.LogAnswer();

			this_.device.PostConnectAck(new ConnectAck((uint)this_.LineNumber));
		}

		/// <summary>
		/// Handles CallState release event. The CallManager is trying to clear the
		/// call. Inform the application.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCallStateRelease(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.LogHangup(event_.Id);

			// Inform the appication that the CallManager wants to clear the call.
			this_.SendReleaseToConsumer();
		}

		/// <summary>
		/// Handles CallState event. Perform actions applicable for the
		/// CallState or translate the event to a more usable call event.
		/// </summary>
		/// <remarks>
		/// Since the resulting CallStateX event is synonymous with the
		/// original CallState event, we need to skip to the beginning of the
		/// line, so to speak, and add the CallStateX events to the front of
		/// the event queue.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for CallState.</param>
		/// <returns>CallStateX as followup event (rather than re-enqueuing
		/// the message).</returns>
		[Action()]
		private static void HandleCallState_(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			CallState message = event_.EventMessage as CallState;

			// Copy the precedence data. CallManager controls the values so just
			// overwrite the current values.
			this_.precedence = message.precedence.precedence;

			switch (message.callState) {
			case CallState.State.Offhook:
				// Features like transfer (while call is idle) open a new call
				// plane starting with the CallState(offhook) message.
				followupEvents.Enqueue(
					new Event((int)EventType.CallStateOffhook, this_));
				break;

			case CallState.State.Onhook:
			// CallRemoteMultiline received when other phone on shared line
			// picks up. We treat that as if the caller hung up on this device.
			case CallState.State.CallRemoteMultiline:
				followupEvents.Enqueue(
					new Event((int)EventType.CallStateRelease, this_));
				break;

			case CallState.State.RingOut:
				followupEvents.Enqueue(
					new Event((int)EventType.CallStateAlerting, this_));
				break;

			case CallState.State.RingIn:
				followupEvents.Enqueue(
					new Event((int)EventType.CallStateSetup, this_));
				break;

			case CallState.State.Connected:
				followupEvents.Enqueue(
					new Event((int)EventType.CallStateConnected, this_));
				break;

			case CallState.State.Proceed:
				followupEvents.Enqueue(
					new Event((int)EventType.CallStateProceeding, this_));
				break;
		        
			case CallState.State.Hold:
				this_.device.PostFeatureRequest(new FeatureRequest((uint)this_.LineNumber,
					FeatureRequest.Feature.Hold));

				// (FeatureResponse events are ignored, so don't add the event
				// to the queue.)
				break;

			default:
				// Do nothing.
				break;
			}
		}

		/// <summary>
		/// Handles CallState.Onhook by releasing call and cleaning up.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCallStateOnhookRelease(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.SendReleaseCompleteToConsumer(ReleaseComplete.Cause.Normal,
				ref followupEvents);

			followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
		}

		/// <summary>
		/// Send digits (dial the phone number).
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSendDigits(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			// Acknowledge the new outgoing call.
			this_.device.PostSetupAck(new SetupAck((uint)this_.LineNumber));

			// Send digits if the application has supplied them.
			if (this_.digits != null && this_.digits.Length > 0)
			{
				foreach (char c in this_.digits)
				{
					this_.Send(new KeypadButton(AsciiDigitToButton(c),
						(uint)this_.LineNumber, (uint)this_.Id));
				}
				this_.digits = null;
			}
			else
			{
				this_.log.Write(TraceLevel.Warning,
					"Cal: {0}: offhook but no digits ({1}); could not send digits",
					this_, this_.digits);
			}
		}

		/// <summary>
		/// Handles OpenReceiveResponse event. The stack has requested an
		/// OpenReceiveChannel. Send OpenReceiveChannelAck to the CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for OpenReceiveResponse.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleOpenReceiveResponse(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Debug.Assert(stateMachine is Call, "SccpStack: stateMachine not Call");
			Debug.Assert(event_ != null, "SccpStack: event_ is null");
			Debug.Assert(event_.EventMessage != null, "SccpStack: EventMessage is null");
			Debug.Assert(event_.EventMessage is OpenReceiveResponse, "SccpStack: EventMessage not OpenReceiveResponse");

			Call this_ = stateMachine as Call;

			Debug.Assert(this_.connection != null, "SccpStack: connection is null");

			OpenReceiveResponse message =
				event_.EventMessage as OpenReceiveResponse;

			Debug.Assert(message.media != null, "SccpStack: media is null");

            OpenReceiveChannelAck.Status status =
				message.cause == Device.Cause.Ok ?
				OpenReceiveChannelAck.Status.Ok : OpenReceiveChannelAck.Status.Error;

			this_.Send(new OpenReceiveChannelAck(status,
				message.media.address, message.media.passthruPartyId, (uint)this_.Id));
		}

		/// <summary>
		/// Handles FeatureRequest event. The application has requested a specific
		/// feature.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for FeatureRequest.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleFeatureRequest_(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			FeatureRequest message = event_.EventMessage as FeatureRequest;
		    
			this_.Send(new SoftkeyEvent((uint)message.feature,
				(uint)this_.LineNumber, (uint)this_.Id));

			// (FeatureResponse events are ignored, so don't add the event to
			// the queue.)
		}

		/// <summary>
		/// Handles connect event. The application has answered an incoming call.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleConnect(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(new OffhookSccp((uint)this_.LineNumber, (uint)this_.Id));
		}

		/// <summary>
		/// Handles Release event from the application or Onhook event from the
		/// CallManager. If Release, the application is attempting to release
		/// a new outgoing call before it is actually placed (before digits are
		/// dialed. Basically, offhook immediately followed by onhook. Send
		/// onhook to the CallManager and cleanup.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Release or Onhook.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleReleaseInitiated(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.LogHangup(event_.Id);

			this_.Send(new Onhook((uint)this_.LineNumber, (uint)this_.Id));

			// Application has rejected the new outgoing call, so clean up.
			followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
		}

		/// <summary>
		/// Handles release event. If Release event, the application is
		/// attempting to clear the call. Send onhook to the CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Release or Onhook.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRelease(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.LogHangup(event_.Id);

			this_.Send(new Onhook((uint)this_.LineNumber, (uint)this_.Id));
		}

		/// <summary>
		/// Handles ReleaseComplete event from the application. The application
		/// is attempting to release a new incoming call. Send onhook to the
		/// CallManager and cleanup.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleIaReleaseComplete(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.LogHangup(event_.Id);

			this_.Send(new Onhook((uint)this_.LineNumber, (uint)this_.Id));
		    
			// Application has rejected the new incoming call, so clean up.
			followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
		}

		/// <summary>
		/// Handles release event from the application or CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Release or Onhook.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleOrRelease(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.LogHangup(event_.Id);

			this_.SendReleaseCompleteToConsumer(ReleaseComplete.Cause.Normal,
				ref followupEvents);

			this_.Send(new Onhook((uint)this_.LineNumber, (uint)this_.Id));
		    
			followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
		}

		/// <summary>
		/// Handles media events from the CallManager. Inform the application of
		/// the event.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for an SccpMessage.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleMedia(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			SccpMessage msg = event_.EventMessage as SccpMessage;

			this_.LogVerbose("Cal: {0}: media message: {1}", this_, msg);

			switch (msg.MessageType)
			{
				case SccpMessage.Type.OpenReceiveChannel:
				{
					OpenReceiveChannel message = msg as OpenReceiveChannel;

					MediaInfo mediaInfo = new MediaInfo(null,
						message.conferenceId,
						message.passthruPartyId, message.packetSize,
						message.payload, message.qualifier.g723BitRate,
						message.qualifier.echoCancellation, 0, false, 0);

					this_.device.PostOpenReceiveRequest(
						new OpenReceiveRequest((uint)this_.LineNumber, mediaInfo));
					break;
				}

				case SccpMessage.Type.CloseReceiveChannel:
				{
					CloseReceiveChannel message = msg as CloseReceiveChannel;

					this_.device.PostCloseReceive(new CloseReceive((uint)this_.LineNumber,
						message.conferenceId, message.passthruPartyId));
					break;
				}

				case SccpMessage.Type.StartMediaTransmission:
				{
					StartMediaTransmission message =
						msg as StartMediaTransmission;

					MediaInfo mediaInfo = new MediaInfo(message.address,
						message.conferenceId, message.passthruPartyId,
						message.packetSize, message.payload,
						message.qualifier.g723BitRate, false,
						message.qualifier.precedence,
						message.qualifier.silenceSuppression,
						message.qualifier.maxFramesPerPacket);

					this_.device.PostStartTransmit(
						new StartTransmit((uint)this_.LineNumber, mediaInfo));
					break;
				}

				case SccpMessage.Type.StopMediaTransmission:
				{
					StopMediaTransmission message = msg as StopMediaTransmission;

					this_.device.PostStopTransmit(new StopTransmit((uint)this_.LineNumber,
						message.conferenceId, message.passthruPartyId));
					break;
				}

				default:
					// Do nothing.
					break;
			}
		}

		/// <summary>
		/// Handles various UI events. 
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for an SccpMessage.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleUpdateUi(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			SccpMessage msg = event_.EventMessage as SccpMessage;

			this_.LogVerbose("Cal: {0}: update-UI message: {1}", this_, msg);

			switch (msg.MessageType)
			{
				case SccpMessage.Type.CallInfo:
				{
					// We may have been waiting for some info contained in this
					// CallInfo SCCP message before we can send a client
					// message to the consumer. If so, mark that we aren't
					// waiting anymore, extract the data we need, and send
					// message to consumer.

					CallInfo message = msg as CallInfo;

					switch (this_.waiting)
					{
						case WaitingToSend.Setup:
							this_.waiting = WaitingToSend.Nothing;

							this_.LogReceivingCall(message.calledPartyNumber,
								message.originalCalledPartyNumber,
								message.callingPartyNumber,
								message.callingPartyName);

							this_.device.PostSetup(new Setup((uint)this_.LineNumber,
								message.calledPartyNumber,
								message.originalCalledPartyNumber,
								message.callingPartyNumber,
								message.callingPartyName));
							break;

						case WaitingToSend.Proceeding:
							this_.waiting = WaitingToSend.Nothing;

							this_.device.PostProceeding(
								new Proceeding((uint)this_.LineNumber));
							break;

						case WaitingToSend.Alerting:
							this_.waiting = WaitingToSend.Nothing;

							this_.device.PostAlerting(
								new Alerting((uint)this_.LineNumber));
							break;

						default:
							// Do nothing.
							break;
					}
					break;
				}

				case SccpMessage.Type.ConnectionStatisticsReq:
				{
					ConnectionStatisticsReq message =
						msg as ConnectionStatisticsReq;

					// Let's return a dummy set of connection statistics.
					this_.Send(new ConnectionStatisticsRes(message.directoryNumber,
						(uint)this_.Id, message.processingMode, 0, 0, 0, 0, 0, 0, 0));
					break;
				}

				default:
					// Do nothing.
					// These are message we do not currently care about.
					break;
			}
		}

		/// <summary>
		/// Handles DeviceToUserDataRequest event from the application. Send the
		/// data to the CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for DeviceToUserDataRequest.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleDeviceToUserDataRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			DeviceToUserDataRequest message =
				event_.EventMessage as DeviceToUserDataRequest;

			if (this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				this_.Send(new DeviceToUserData(message.data.applicationId,
					(uint)this_.LineNumber, (uint)this_.Id, message.data.transactionId,
					message.data.data));
			}
		}


		/// <summary>
		/// Handles DeviceToUserDataResponse event from the application. Send
		/// the response to the CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for DeviceToUserDataResponse.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleDeviceToUserDataResponse(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			DeviceToUserDataResponse message =
				event_.EventMessage as DeviceToUserDataResponse;

			if (this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				this_.Send(new DeviceToUserDataRes(message.data.applicationId,
					(uint)this_.LineNumber, (uint)this_.Id, message.data.transactionId,
					message.data.data));
			}
		}

		#region Support methods to the HandleX methods

		/// <summary>
		/// Translates from ASCII DTMF digit to KeypadButton.Button enumeration.
		/// </summary>
		/// <param name="digit">Digit to translate.</param>
		/// <returns>Button enumeration.</returns>
		private static KeypadButton.Button AsciiDigitToButton(char digit)
		{
			KeypadButton.Button button;

			switch (digit)
			{
				default:		// Just to set it to something if invalid.
				case '0':
					button = KeypadButton.Button.Zero;
					break;
				case '1':
					button = KeypadButton.Button.One;
					break;
				case '2':
					button = KeypadButton.Button.Two;
					break;
				case '3':
					button = KeypadButton.Button.Three;
					break;
				case '4':
					button = KeypadButton.Button.Four;
					break;
				case '5':
					button = KeypadButton.Button.Five;
					break;
				case '6':
					button = KeypadButton.Button.Six;
					break;
				case '7':
					button = KeypadButton.Button.Seven;
					break;
				case '8':
					button = KeypadButton.Button.Eight;
					break;
				case '9':
					button = KeypadButton.Button.Nine;
					break;
				case 'a':
					button = KeypadButton.Button.A;
					break;
				case 'A':
					button = KeypadButton.Button.A;
					break;
				case 'b':
					button = KeypadButton.Button.B;
					break;
				case 'B':
					button = KeypadButton.Button.B;
					break;
				case 'c':
					button = KeypadButton.Button.C;
					break;
				case 'C':
					button = KeypadButton.Button.C;
					break;
				case 'd':
					button = KeypadButton.Button.D;
					break;
				case 'D':
					button = KeypadButton.Button.D;
					break;
				case '*':
					button = KeypadButton.Button.Star;
					break;
				case '#':
					button = KeypadButton.Button.Pound;
					break;
			}

			return button;
		}

		/// <summary>
		/// Translates from KeypadButton.Button enumeration to ASCII DTMF digit.
		/// </summary>
		/// <param name="button">Button to translate.</param>
		/// <returns>ASCII DTMF digit.</returns>
		private static char ButtonToAsciiDigit(KeypadButton.Button button)
		{
			char digit;

			switch (button)
			{
				default:		// Just to set it to something if invalid.
				case KeypadButton.Button.Zero:
					digit = '0';
					break;
				case KeypadButton.Button.One:
					digit = '1';
					break;
				case KeypadButton.Button.Two:
					digit = '2';
					break;
				case KeypadButton.Button.Three:
					digit = '3';
					break;
				case KeypadButton.Button.Four:
					digit = '4';
					break;
				case KeypadButton.Button.Five:
					digit = '5';
					break;
				case KeypadButton.Button.Six:
					digit = '6';
					break;
				case KeypadButton.Button.Seven:
					digit = '7';
					break;
				case KeypadButton.Button.Eight:
					digit = '8';
					break;
				case KeypadButton.Button.Nine:
					digit = '9';
					break;
				case KeypadButton.Button.A:
					digit = 'A';
					break;
				case KeypadButton.Button.B:
					digit = 'B';
					break;
				case KeypadButton.Button.C:
					digit = 'C';
					break;
				case KeypadButton.Button.D:
					digit = 'D';
					break;
				case KeypadButton.Button.Star:
					digit = '*';
					break;
				case KeypadButton.Button.Pound:
					digit = '#';
					break;
			}

			return digit;
		}

		/// <summary>
		/// Asks consumer to release call by sending ReleaseComplete back down
		/// to stack.
		/// </summary>
		private void SendReleaseToConsumer()
		{
			device.PostRelease(new Release((uint)LineNumber));
		}

		/// <summary>
		/// Tells consumer that call is being released, remove this call from
		/// the stack, and, if the stack requested a reset and this is the last
		/// call, let's start the reset.
		/// </summary>
		/// <param name="cause">What caused this ReleaseComplete to be sent.</param>
		/// <param name="followupEvents">Followup events.</param>
		private void SendReleaseCompleteToConsumer(ReleaseComplete.Cause cause,
			ref Queue followupEvents)
		{
			device.PostReleaseComplete(new ReleaseComplete((uint)LineNumber, cause));

			device.Calls.Remove(this);

			if (device.Calls.IsEmpty &&
				device.State == Device.DeviceState.Resetting)
			{
				followupEvents.Enqueue(new Event((int)Discovery.EventType.ResetRequest,
					new CloseDeviceRequest(device.InternalResetCause),
					device.Discoverer));
			}
		}

		/// <summary>
		/// Sends Offhook to the consumer.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void SendOffhookToConsumer(ref Queue followupEvents)
		{
			// (Save local copy in case changes within this method. Don't
			// think it even can, though.)
			uint localLineNumber = (uint)LineNumber;

			if (device.State != Device.DeviceState.Registered)
			{
				followupEvents.Enqueue(new Event((int)EventType.ReleaseComplete,
					new ReleaseComplete(localLineNumber, ReleaseComplete.Cause.NotRegistered),
					this));
			}

			device.PostOffhookClient(new OffhookClient(localLineNumber));
		}

		/// <summary>
		/// Prepares to send Setup up to the consumer.
		/// </summary>
		/// <param name="id">Call identifier.</param>
		/// <param name="line">Line number.</param>
		/// <param name="connInfo">Connection information.</param>
		/// <param name="media">Media information.</param>
		/// <param name="followupEvents">Followup events.</param>
		private void PrepareToSendSetupToConsumer(int id, int line,
			ConnInfo connInfo, MediaInfo media, ref Queue followupEvents)
		{
			if (device.State != Device.DeviceState.Registered)
			{
				log.Write(TraceLevel.Error,
					"Cal: not registered; cannot set new call data (id: {0}, line: {1})",
					id, line);
				followupEvents.Enqueue(new Event((int)EventType.ReleaseComplete,
					new ReleaseComplete((uint)line, ReleaseComplete.Cause.NotRegistered),
					this));
			}
			else
			{
#if false
				// TBD - Nuke this high-level constructor (sapp_calls) because
				// low-level constructor takes care of it (sccpcb->cccbs).
				Call call = new IncomingCall(device, Log, line, id);

				// If Call constructor wasn't also able to add itself to the
				// Calls collection, pretend like it never happened. Messy,
				// but, right or wrong, a call manages its presence in the
				// collection.
				if (!call.Added)
				{
					call = null;
				}
#endif

				// We don't have the calledNumber yet, so we can't send
				// Setup to application. Wait until we receive the ConnInfo
				// and grab the data from that. The waiting flag was set by
				// IncomingCall constructor and will be used when the ConnInfo
				// message is received to indicate that the Setup needs to
				// be sent.
			}
		}

		#region Call-logging methods

		/// <summary>
		/// Logs receiving a call (haven't answered the call yet).
		/// </summary>
		private void LogReceivingCall(string calledPartyNumber,
			string originalCalledPartyNumber, string callingPartyNumber,
			string callingPartyName)
		{
			// Check for weirdness. "Can't happen," but you never know.
			if (callStartTimeNs != Const.HPTimerNotSet)
			{
				log.Write(TraceLevel.Warning,
					"Cal: {0}: receiving call that started {1}ms ago", this,
					(HPTimer.Now() - callStartTimeNs) / 1000 / 1000);
			}

			callStartTimeNs = HPTimer.Now();

			log.Write(TraceLevel.Info, "Cal: {0}: {1}{2} receiving call from {3}{4}",
				this, calledPartyNumber,
				originalCalledPartyNumber == null ||
				originalCalledPartyNumber.Length == 0 ||
				calledPartyNumber == originalCalledPartyNumber ? string.Empty :
				" (nee " + originalCalledPartyNumber.ToString() + ")",
				callingPartyNumber,
				callingPartyName == null || callingPartyName == string.Empty ?
				string.Empty : "(" + callingPartyName.ToString() + ")");
		}

		/// <summary>
		/// Logs placing a call.
		/// </summary>
		private void LogPlacingCall(string callingPartyNumber, string callingPartyName,
			string calledPartyNumber, string originalCalledPartyNumber)
		{
			// Check for weirdness. "Can't happen," but you never know.
			if (callStartTimeNs != Const.HPTimerNotSet)
			{
				log.Write(TraceLevel.Warning,
					"Cal: {0}: placing call that started {1}ms ago", this,
					(HPTimer.Now() - callStartTimeNs) / 1000 / 1000);
			}

			callStartTimeNs = HPTimer.Now();

			log.Write(TraceLevel.Info, "Cal: {0}: {1}{2} placing call to {3}{4}",
				this,
				callingPartyNumber == null ? "?" : callingPartyNumber.ToString(),
				callingPartyName == null || callingPartyName == string.Empty ?
				string.Empty : "(" + callingPartyName.ToString() + ")",
				calledPartyNumber,
				originalCalledPartyNumber == null ||
				originalCalledPartyNumber.Length == 0 ||
				calledPartyNumber == originalCalledPartyNumber ? string.Empty :
				" (nee " + originalCalledPartyNumber.ToString() + ")");
		}

		/// <summary>
		/// Saves time so that we can log answer-related info later.
		/// </summary>
		private void LogAnswer()
		{
			callAnswerTimeNs = HPTimer.Now();
		}

		/// <summary>
		/// Logs the hangup. Assume that _we_ are hanging up the call if the
		/// event is Release and that, otherwise, we are being hungup on by the
		/// other party.
		/// </summary>
		/// <param name="id">Event id. Typically Release or Onhook.</param>
		private void LogHangup(int id)
		{
			long ringDurationNs;
			long callDurationNs;

			// Check for weirdness. "Can't happen," but you never know.
			if (callStartTimeNs == Const.HPTimerNotSet)
			{
				log.Write(TraceLevel.Warning,
					"Cal: {0}: hanging up inactive call; ring duration unknown",
					this);

				ringDurationNs = 0;

				if (callAnswerTimeNs == Const.HPTimerNotSet)
				{
					// Abandoned call (nobody answered)?
					callDurationNs = 0;
				}
				else
				{
					// Only thing we can determine is call duration.
					callDurationNs = HPTimer.Now() - callAnswerTimeNs;

					callAnswerTimeNs = Const.HPTimerNotSet;	// Indicate not set.
				}
			}
			else
			{
				if (callAnswerTimeNs == Const.HPTimerNotSet)
				{
					// Abandoned call (nobody answered)
					ringDurationNs = HPTimer.Now() - callStartTimeNs;
					callDurationNs = 0;
				}
				else
				{
					// Normal call--someone answered and had a call
					ringDurationNs = callAnswerTimeNs - callStartTimeNs;
					callDurationNs = HPTimer.Now() - callAnswerTimeNs;

					callAnswerTimeNs = Const.HPTimerNotSet;	// Indicate not set.
				}
				callStartTimeNs = Const.HPTimerNotSet;	// Indicate not set.
			}

			log.Write(TraceLevel.Info, "Cal: {0}: {1} hangup; ring: {2}ms, call: {3}ms", this,
				(EventType)id == EventType.Release ? "outgoing" : "incoming",
				ringDurationNs / 1000 / 1000, callDurationNs / 1000 / 1000);
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
					"Cal: {0}: attempt to Send {1} on a null connection; ignored",
					this, message);

				// In case connection is null.
				sent = false;
			}

			return sent;
		}

		/// <summary>
		/// Returns whether we are still connected to a CallManager.
		/// </summary>
		public bool Connected
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
					log.Write(TraceLevel.Warning,
						"Cal: {0}: attempt to determine whether Connected on a null connection; I guess not",
						this);

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
		#endregion

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
		/// <returns>Empty string since this class doesn't have timers.</returns>
		public override string IntToTimerEnumString(int enumValue)
		{
			return string.Empty;
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
			return (connection == null ? "Call" : connection.ToString()) + "-" +
				Id.ToString() + "/" + LineNumber.ToString();
		}
	}

	/// <summary>
	/// Represents an outgoing SCCP call.
	/// </summary>
	internal class OutgoingCall : Call
	{
		// Construct an outgoing-call object.
		internal OutgoingCall(Device device, LogWriter log,
			int lineNumber, int id) :
			base(device, log, lineNumber, id, WaitingToSend.Proceeding) { }
	}

	/// <summary>
	/// Represents an incoming SCCP call.
	/// </summary>
	internal class IncomingCall : Call
	{
		// Construct an incoming-call object.
		internal IncomingCall(Device device, LogWriter log,
			int lineNumber, int id) :
			base(device, log, lineNumber, id, WaitingToSend.Setup) { }
	}
}
