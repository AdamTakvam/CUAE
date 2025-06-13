using System;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Threading;

using Metreos.SccpStack;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.CallControl.Sccp;

namespace Metreos.CallControl.Sccp
{
	/// <summary>
	/// Represents an SCCP call mainly by use of a state machine.
	/// </summary>
	[StateMachine(ActionPrefix="Handle", StateDefinitionPrefix="Define")]
	public abstract class Call : StateMachine
	{
		/// <summary>
		/// Constructs a Call without display name (defaults to empty string).
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		public Call(SccpProvider provider, LogWriter log, long callId,
			string to, string from, string originalTo) :
			this(provider, log, callId, to, from, originalTo,
			string.Empty) { }

		/// <summary>
		/// Constructs a Call.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		/// <param name="displayName">Display name of caller.</param>
		public Call(SccpProvider provider, LogWriter log, long callId,
			string to, string from, string originalTo, string displayName) :
			base(log, ref control)
		{
			this.provider = provider;
			this.callId = callId;
			this.to = to;
			this.from = from;
			this.originalTo = originalTo == null ? to : originalTo;
			this.displayName = displayName;
			miscTimer = new EventTimer(log);

			// null means that the Telephony Manager has not yet invoked the
			// SetMedia action for this Call.
			media = null;

			LogVerbose("Prv: {0}: created Call", this);
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
		public Call(LogWriter log, ref StateMachineStaticControl control) :
			base(log, ref control) { }

		/// <summary>
		/// Object through which StateMachine assures that static state machine
		/// is constructed only once.
		/// </summary>
		private static StateMachineStaticControl control = new StateMachineStaticControl();

		/// <summary>
		/// SccpProvider.
		/// </summary>
		protected readonly SccpProvider provider;

		/// <summary>
		/// Device on which this Call is active.
		/// </summary>
		private Device device;

		/// <summary>
		/// Device on which this Call is active.
		/// </summary>
		public Device Device { get { return device; } set { device = value; } }

		/// <summary>
		/// Media capabilities saved from a SetMedia action from the Telephony
		/// Manager.
		/// </summary>
		private MediaInfo media;

		/// <summary>
		/// Pass-through party id. (Transaction identifier.)
		/// </summary>
		/// <remarks>From OpenReceiveRequest. We eventually return it in the
		/// OpenReceiveResponse.</remarks>
		private uint passthruPartyId;

		/// <summary>
		/// Uniquely identifies this call for use between the Telephony Manager
		/// and the provider.
		/// </summary>
		protected readonly long callId;

		/// <summary>
		/// Property whose value is this Call's unique identifier for use
		/// between the Telephony Manager and the provider.
		/// </summary>
		public long CallId { get { return callId; } }

		/// <summary>
		/// Directory number being called.
		/// </summary>
		private readonly string to;

		/// <summary>
		/// Directory number from which this call originates.
		/// </summary>
		private readonly string from;

		/// <summary>
		/// Original directory number being called in case of a forwarded call.
		/// </summary>
		private readonly string originalTo;

		/// <summary>
		/// Display name of caller or empty string if not specified.
		/// </summary>
		private readonly string displayName;

		/// <summary>
		/// Tell the stack to initiate an outgoing call.
		/// </summary>
		/// <param name="to">Directory number of the party we are
		/// calling.</param>
		/// <param name="followupEvents">Followup events.</param>
		protected abstract void InitiateCallWithStack(string to, ref Queue followupEvents);

		/// <summary>
		/// Property whose value is whether this call expects to receive a
		/// Setup message from the stack during the call.
		/// </summary>
		public abstract bool SetupExpectedDuringCall { get; }

		/// <summary>
		/// Timer used for various purposes (we only waiting for one thing at
		/// a time).
		/// </summary>
		/// <remarks>
		/// Access control not needed here because underlying class provides it.
		/// </remarks>
		private readonly EventTimer miscTimer;

		#region Finite State Machine

		#region State declarations
		private static State callInitiated = null;
		private static State callInitiatedOutgoingCallPending = null;
		private static State waitForFirstMedia = null;
		private static State mediaSet = null;
		private static State mediaSetOutgoingCallPending = null;
		private static State openReceiveRequested = null;
		private static State incomingMediaEstablished = null;
		private static State outgoingMediaEstablished = null;
		private static State outgoingMediaEstablishedWithMediaSet = null;
		private static State outgoingMediaEstablishedWithOpenReceiveRequested = null;
		
		private static State mediaEstablished = null;
		private static State mediaEstablishedMohDisabled = null;

		private static State requestHold = null;
		private static State listeningToMoh = null;
		private static State noMedia = null;
		#endregion

		/// <summary>
		/// Types of events that can trigger actions within this state machine.
		/// </summary>
		public enum EventType
		{
			WaitForFirstMedia,	// Internal (but indirectly from TM)
			SetMedia,			// From TM
			UseMohMedia,		// From TM
			DontUseMohMedia,	// Internal
			ProceedFromPending,	// Internal
			ClearMedia,			// From TM; SetMedia(null)
			StartHoldTone,		// Internal (via StartTone from CCM)
			StopTone,			// From CCM
			OpenReceiveRequest,	// From CCM
			StartTransmit,		// From CCM
			StopTransmit,		// From CCM
			Accept,				// From TM
			Answer,				// From TM
			Redirect,			// From TM
			Reject,				// From TM
			SendUserInput,		// From TM
			Hangup,				// From TM
			ReceivedAlerting,	// From CCM
			ReceivedConnect,	// From CCM
			ReceivedConnectAck,	// From CCM
			ReceivedRelease,	// From CCM
			ReceivedReleaseComplete,	// From CCM
			ReceivedDigits,		// From CCM
			Timeout,			// Internal
			CallInfo,			// From CCM
			Hold,				// From TM
			Resume,				// From TM
			MohDisabled,		// From TM
			MohEnabled,			// From TM
		}

		#region Action declarations
		private static ActionDelegate SaveMedia = null;
		private static ActionDelegate PlaceCall = null;
		private static ActionDelegate PlaceCallNoMedia = null;
		private static ActionDelegate SetMediaAndRespond = null;
		private static ActionDelegate Resume = null;
		private static ActionDelegate ClearMedia = null;
		private static ActionDelegate Hold = null;
		private static ActionDelegate StartHoldTone = null;
		private static ActionDelegate StopHoldTone = null;
		private static ActionDelegate PendingOpenReceiveRequest = null;
		private static ActionDelegate RespondToOpenReceiveRequest = null;
		private static ActionDelegate RespondToOpenReceiveRequestWithDummy = null;
		private static ActionDelegate EstablishMedia = null;
		private static ActionDelegate ChangeMedia = null;
		private static ActionDelegate EstablishNoMedia = null;
		private static ActionDelegate ChangeToNoMedia = null;
		private static ActionDelegate Accept = null;
		private static ActionDelegate Answer = null;
		private static ActionDelegate Redirect = null;
		private static ActionDelegate Reject = null;
		private static ActionDelegate SendUserInput = null;
		private static ActionDelegate Hangup = null;
		private static ActionDelegate ReceivedConnect = null;
		private static ActionDelegate ReceivedConnectAck = null;
		private static ActionDelegate ReceivedRelease = null;
		private static ActionDelegate ReceivedReleaseComplete = null;
		private static ActionDelegate ReceivedReleaseConsiderFailover = null;
		private static ActionDelegate ReceivedReleaseCompleteConsiderFailover = null;
		private static ActionDelegate ReceivedDigits = null;
		private static ActionDelegate SavePassthroughId = null;
		private static ActionDelegate HoldAndSendMediaChanged = null;

		private static ActionDelegate EstablishMediaAndSendCallEstablished = null;
		private static ActionDelegate SendConnectAck = null;

		private static ActionDelegate SetMediaAndRespondAndCheckMoh = null;
		private static ActionDelegate EstablishMediaAndCheckMoh = null;
		private static ActionDelegate RespondToOpenReceiveRequestAndCheckMoh = null;

		private static ActionDelegate DoNothing = null;
		
		#endregion

		#region State-definition methods

		// The goal of this cluster of states is to eventually transition to
		// the mediaEstablished state. Once there, we never transition back to
		// this cluster.
		#region Call-setup state cluster

		/// <summary>
		/// Defines the callInitiated state where either the call has just
		/// been constructed and nothing else has happened yet or--extremly
		/// unlikely--the state machine has progressed through other states but
		/// subsequent events have undone this progress and we are back to the
		/// beginning.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State(Initial=true)]
		private static void DefineCallInitiated(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.WaitForFirstMedia,							waitForFirstMedia);	// Outgoing calls start here
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia,				mediaSet);
			state.Add(EventType.ClearMedia,			ClearMedia);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	PendingOpenReceiveRequest,
				openReceiveRequested);
			state.Add(EventType.StartTransmit,		EstablishMedia,			outgoingMediaEstablished);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the callInitiatedoutgoingCallPending state where an outgoing
		/// P2P call has just been constructed, we have sent Setup to the
		/// stack, but nothing else has happened yet--the outgoing call is
		/// pending.
		/// </summary>
		/// <remarks>
		/// This is a special case of the callInitiated state for P2P outgoing
		/// calls where the call has just begun by sending Setup to the stack
		/// but nothing else has happended to change the state of the Telephony
		/// Manager. It is used for failover processing--if Release or
		/// ReleaseComplete is encountered while in this state, failover to
		/// another device is attempted if warranted and possible.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineCallInitiatedOutgoingCallPending(State state)
		{
			state.Add(EventType.ReceivedAlerting,							callInitiated);
			state.Add(EventType.ReceivedConnect,	ReceivedConnect,		callInitiated);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck,		callInitiated);
			state.Add(EventType.Accept,				Accept,					callInitiated);
			state.Add(EventType.Answer,				Answer,					callInitiated);
			state.Add(EventType.Redirect,			Redirect,				callInitiated);
			state.Add(EventType.Reject,				Reject,					callInitiated);
			state.Add(EventType.SetMedia,			SaveMedia,				mediaSet);
			state.Add(EventType.ClearMedia,			ClearMedia,				callInitiated);
			state.Add(EventType.StartHoldTone,		StartHoldTone,			callInitiated);
			state.Add(EventType.StopTone,			StopHoldTone,			callInitiated);
			state.Add(EventType.OpenReceiveRequest,	PendingOpenReceiveRequest,
				openReceiveRequested);
			state.Add(EventType.StartTransmit,		EstablishMedia,			outgoingMediaEstablished);
			state.Add(EventType.SendUserInput,		SendUserInput,			callInitiated);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits,			callInitiated);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedReleaseConsiderFailover);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseCompleteConsiderFailover);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the waitForFirstMedia state where outgoing calls
		/// effectively start while we wait for the Telephony Manager to tell
		/// us with the SetMedia action what address to report to the stack in
		/// OpenReceiveResponse for incoming media.
		/// </summary>
		/// <remarks>
		/// Once we get the address, we initiate the call. We wait to initiate
		/// the call because we could otherwise receive OpenReceiveRequest from
		/// the stack and be unable to respond because we don't know what
		/// address to use.
		/// 
		/// The call object is always constructed starting in the callInitiated
		/// state, but the state machine is immediately nudged into this state
		/// for outgoing calls.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineWaitForFirstMedia(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.SetMedia,			PlaceCall,				mediaSetOutgoingCallPending);
			state.Add(EventType.ClearMedia,			PlaceCallNoMedia,		callInitiatedOutgoingCallPending);	// P2P call
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.Hangup,				Hangup);					// We never actually initiated the call
			state.Add(EventType.CallInfo);
		}

		// This cluster of states handles the general scenario where incoming
		// media is established before outgoing media. For example,
		// ORReq, SetMedia, StartTransmit
		// SetMedia, ORReq, StartTransmit
		#region Incoming-media-established state cluster

		/// <summary>
		/// Defines the mediaSetOutgoingCallPending state where the Telephony
		/// Manager has invoked SetMedia for the first time. We are now
		/// prepared to respond to an OpenReceiveRequest from the stack.
		/// </summary>
		/// <remarks>
		/// This is a special case of the mediaSet state for outgoing calls
		/// where the call has just been initiated with the stack but nothing
		/// else has happended to change the state of the Telephony Manager. It
		/// is used for failover processing--if Release or ReleaseComplete is
		/// encountered while in this state, failover to another device is
		/// attempted if warranted and possible.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineMediaSetOutgoingCallPending(State state)
		{
			state.Add(EventType.ReceivedAlerting,							mediaSet);
			state.Add(EventType.ReceivedConnect,	ReceivedConnect,		mediaSet);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck,		mediaSet);
			state.Add(EventType.Accept,				Accept,					mediaSet);
			state.Add(EventType.Answer,				Answer,					mediaSet);
			state.Add(EventType.Redirect,			Redirect,				mediaSet);
			state.Add(EventType.Reject,				Reject,					mediaSet);
			state.Add(EventType.SetMedia,			SaveMedia,				mediaSet);
			state.Add(EventType.ClearMedia,			ClearMedia,				callInitiated);
			state.Add(EventType.StartHoldTone,		StartHoldTone,			mediaSet);
			state.Add(EventType.StopTone,			StopHoldTone,			mediaSet);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequest,
				incomingMediaEstablished);
			state.Add(EventType.StartTransmit,		EstablishMedia,			outgoingMediaEstablishedWithMediaSet);
			state.Add(EventType.SendUserInput,		SendUserInput,			mediaSet);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits,			mediaSet);
			state.Add(EventType.ProceedFromPending,							mediaSet);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedReleaseConsiderFailover);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseCompleteConsiderFailover);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the mediaSet state where the Telephony Manager has either
		/// invoked SetMedia for the first time or--extremly unlikely--the
		/// state machine has progressed through other states but subsequent
		/// events have undone this progress and we are back here. We are now
		/// prepared to respond to an OpenReceiveRequest from the stack.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineMediaSet(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia);
			state.Add(EventType.ClearMedia,			ClearMedia,				callInitiated);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequest,
				incomingMediaEstablished);
			state.Add(EventType.StartTransmit,		EstablishMedia,			outgoingMediaEstablishedWithMediaSet);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedReleaseConsiderFailover);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseCompleteConsiderFailover);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the openReceiveRequested state where we have received
		/// OpenDeviceRequest from the stack but are waiting for the Telephony
		/// Manager to invoke the SetMedia action before we can respond.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineOpenReceiveRequested(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	SendConnectAck);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SetMediaAndRespond,		incomingMediaEstablished);
			state.Add(EventType.ClearMedia,			ClearMedia);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	PendingOpenReceiveRequest);
			state.Add(EventType.StartTransmit,		EstablishMediaAndSendCallEstablished,			outgoingMediaEstablishedWithOpenReceiveRequested);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the incomingMediaEstablished state where we have responded
		/// to an OpenReceiveRequest from the stack but have not yet started
		/// transmitting media because we have not received StartTransmit from
		/// the stack--we haven't been told where to send media.
		/// </summary>
		/// <remarks>
		/// This state is analogous to the outgoingMediaEstablished state--they
		/// both represent media having been established in just one
		/// direction. The state machine transitions into this state due to the
		/// the second event in the following typical event sequences.
		/// 
		/// ORReq, SetMedia, StartTransmit
		/// SetMedia, ORReq, StartTransmit
		/// 
		/// This state, incomingMediaEstablishedWithMediaSet, and
		/// incomingMediaEstablishedWithOpenReceiveRequested can all be thought
		/// of as substates of a theoretical inomcing-media-established
		/// superstate.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineIncomingMediaEstablished(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia);
			state.Add(EventType.ClearMedia,			ClearMedia);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequest);
			state.Add(EventType.StartTransmit,		EstablishMediaAndCheckMoh,			mediaEstablished);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.CallInfo);
		}

		#endregion

		// This cluster of states handles the general scenario where outgoing
		// media is established before incoming media. For example,
		// StartTransmit, ORReq, SetMedia
		// StartTransmit, SetMedia, ORReq
		#region Outgoing-media-established state cluster

		/// <summary>
		/// Defines the outgoingMediaEstablished state where we have received
		/// StartTransmit from the stack (so we know where to send media)
		/// without having first set up the incoming-media side.
		/// </summary>
		/// <remarks>
		/// I don't remember ever seeing this happen. The CallManager always
		/// sends OpenReceiveChannel then waits for the OpenReceiveChannelAck
		/// before it sends StartMediaTransmission so that it knows what
		/// address to provide in the StartMediaTransmission that it sends to
		/// the other client.
		/// 
		/// This state is analogous to the incomingMediaEstablished state--they
		/// both represent media having been established in just one
		/// direction. The state machine transitions into this state due to the
		/// the first event in the following typical event sequences.
		/// 
		/// StartTransmit, ORReq, SetMedia
		/// StartTransmit, SetMedia, ORReq
		/// 
		/// This state, outgoingMediaEstablishedWithMediaSet, and
		/// outgoingMediaEstablishedWithOpenReceiveRequested can all be thought
		/// of as substates of a theoretical outgoing-media-established
		/// superstate, or state cluster.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineOutgoingMediaEstablished(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia,				outgoingMediaEstablishedWithMediaSet);
			state.Add(EventType.ClearMedia,			ClearMedia);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	PendingOpenReceiveRequest,
				outgoingMediaEstablishedWithOpenReceiveRequested);
			state.Add(EventType.StartTransmit,		EstablishMedia);
			state.Add(EventType.StopTransmit,		EstablishNoMedia,		callInitiated);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the outgoingMediaEstablishedWithMediaSet state where the
		/// Telephony Manager has invoked the SetMedia action and we have
		/// received StartTransmit from the stack (in either order) but we have
		/// not yet received OpenReceiveRequest from the stack.
		/// </summary>
		/// <remarks>
		/// This state is analogous to the mediaSet state--they
		/// both represent the Telephony Manager having invoked the SetMedia
		/// action before we have received OpenReceiveRequest from the stack.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineOutgoingMediaEstablishedWithMediaSet(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia);
			state.Add(EventType.ClearMedia,			ClearMedia,				outgoingMediaEstablished);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequestAndCheckMoh,
				mediaEstablished);
			state.Add(EventType.StartTransmit,		EstablishMedia);
			state.Add(EventType.StopTransmit,		EstablishNoMedia,		mediaSet);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the outgoingMediaEstablishedWithOpenReceiveRequested state where .
		/// </summary>
		/// <remarks>
		/// This state is analogous to the openReceiveRequested state--they
		/// both represent having received OpenReceiveRequest from the stack
		/// but we're still waiting for the Telephony Manager to invoke the
		/// SetMedia action so that we can respond with OpenReceiveResponse.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineOutgoingMediaEstablishedWithOpenReceiveRequested(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SetMediaAndRespondAndCheckMoh,		mediaEstablished);
			state.Add(EventType.ClearMedia,			ClearMedia);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	PendingOpenReceiveRequest);
			state.Add(EventType.StartTransmit,		EstablishMedia);
			state.Add(EventType.StopTransmit,		EstablishNoMedia,			openReceiveRequested);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.CallInfo);
		}

		#endregion

		#endregion

		// We never transition out of this cluster of states--once we
		// transition to the mediaEstablished state, from then on we just
		// transition between the states in this cluster until the Call is
		// terminated and the state machine is no more (there is no terminal
		// state).
		#region Media-established state cluster

		/// <summary>
		/// Defines the mediaEstablished state where media is established in
		/// both directions.
		/// </summary>
		/// <remarks>
		/// To reach this state, the state machine had to have encountered
		/// these events in any order (this is the typical order):
		/// 
		/// SetMedia, ORReq, StartTransmit
		/// 
		/// From here, the Call can be terminated (there is no terminal state),
		/// the Telephony Manager can put the Call on hold, or we can receive
		/// a StopTransmit from the stack (the other phone has probably been
		/// put on hold).
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineMediaEstablished(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia);
			state.Add(EventType.ClearMedia,			Hold,					requestHold);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequest);
			state.Add(EventType.StartTransmit,		ChangeMedia);
			state.Add(EventType.StopTransmit,		ChangeToNoMedia,	noMedia);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.Hold,				HoldAndSendMediaChanged,	requestHold);
			state.Add(EventType.CallInfo);
			state.Add(EventType.UseMohMedia);
			state.Add(EventType.MohEnabled);
			state.Add(EventType.MohDisabled,		DoNothing,			mediaEstablishedMohDisabled);
		}

		/// <summary>
		/// Defines the mediaEstablished state where media is established in
		/// both directions.
		/// </summary>
		/// <remarks>
		/// To reach this state, the state machine had to have encountered
		/// these events in any order (this is the typical order):
		/// 
		/// SetMedia, ORReq, StartTransmit
		/// 
		/// From here, the Call can be terminated (there is no terminal state),
		/// the Telephony Manager can put the Call on hold, or we can receive
		/// a StopTransmit from the stack (the other phone has probably been
		/// put on hold).
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineMediaEstablishedMohDisabled(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia);
			state.Add(EventType.ClearMedia,			Hold,					requestHold);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequest);
			state.Add(EventType.StartTransmit,		ChangeMedia);
			state.Add(EventType.StopTransmit,		ChangeToNoMedia,	listeningToMoh);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.Hold,				HoldAndSendMediaChanged,	requestHold);
			state.Add(EventType.CallInfo);
			state.Add(EventType.UseMohMedia);
			state.Add(EventType.MohEnabled);
			//state.Add(EventType.MohDisabled,		DoNothing,			mediaEstablishedMohDisabled);
		}

		# region hold/resume states for moh enabled cases.
		/// <summary>
		/// Defines the NoMedia state where we have received StopTransmit
		/// from the stack, most likely because the other client has placed the call
		/// on hold.
		/// </summary>
		/// <remarks>
		/// From here, the Call can be terminated (there is no terminal state)
		/// or the Telephony Manager can take the Call off hold (by invoking
		/// SetMedia with an address).
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineNoMedia(State state) 
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SaveMedia);
			state.Add(EventType.ClearMedia,			Hold,					requestHold);
			state.Add(EventType.StartHoldTone,		StartHoldTone, listeningToMoh); //requestedToHold); //AndSendMohDisabled,	listeningToHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone); //AndRemoteHold,	requestedToHold);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequestWithDummy, listeningToMoh);//RespondToOpenReceiveRequest);
			state.Add(EventType.StartTransmit,		ChangeMedia);
			state.Add(EventType.StopTransmit,		ChangeToNoMedia);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.Hold,				HoldAndSendMediaChanged,	requestHold);
			state.Add(EventType.CallInfo);
			state.Add(EventType.UseMohMedia);
		}


		/// <summary>
		/// Defines the ListeningToMoh state where we have received OpenReceiveRequest
		/// from the stack while in RequestedToHold state. Call Manager is asking 
		/// Telephony Manager for a media port to transmit Music-On-Hold to.
		/// </summary>
		/// <remarks>
		/// From here, the Call can be terminated (there is no terminal state)
		/// or the Telephony Manager can take the Call off hold (by invoking
		/// SetMedia with an address).
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineListeningToMoh(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			SetMediaAndRespondAndCheckMoh,					mediaEstablished);
			state.Add(EventType.ClearMedia,			ClearMedia);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest, SavePassthroughId);
			state.Add(EventType.UseMohMedia);
			state.Add(EventType.StartTransmit,		ChangeMedia);
			state.Add(EventType.StopTransmit,		ChangeToNoMedia);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			//state.Add(EventType.Hold,				HoldAndSendMediaChanged, holdBothEnds);
			state.Add(EventType.CallInfo);
		}

		/// <summary>
		/// Defines the hold state where the Telehophony Manager has asked to
		/// hold the call.
		/// </summary>
		/// <remarks>
		/// When it is in this state, the other end of the call should be in RequestedToHold
		/// if it is managed by TM. It can be resumed by a resume request from TM or SetMedia.
		/// It will be transitioned into RequestHoldAfterFirstCallInfo after the CallInfo message
		/// from Call Manager arrives as part of Hold request to Call Manager.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineRequestHold(State state)
		{
			//					Event				Action					Next State
			state.Add(EventType.ReceivedAlerting);								// Ignore in this state.
			state.Add(EventType.ReceivedConnect,	ReceivedConnect);
			state.Add(EventType.ReceivedConnectAck,	ReceivedConnectAck);
			state.Add(EventType.Accept,				Accept);
			state.Add(EventType.Answer,				Answer);
			state.Add(EventType.Redirect,			Redirect);
			state.Add(EventType.Reject,				Reject);
			state.Add(EventType.SetMedia,			Resume);//, requestResume);
			state.Add(EventType.UseMohMedia);
			state.Add(EventType.ClearMedia,			Resume);
			state.Add(EventType.StartHoldTone,		StartHoldTone);
			state.Add(EventType.StopTone,			StopHoldTone);
			state.Add(EventType.OpenReceiveRequest,	RespondToOpenReceiveRequestAndCheckMoh, mediaEstablished);
			state.Add(EventType.StartTransmit,		ChangeMedia);
			state.Add(EventType.StopTransmit);
			state.Add(EventType.SendUserInput,		SendUserInput);
			state.Add(EventType.ReceivedDigits,		ReceivedDigits);
			state.Add(EventType.Hangup,				Hangup);
			state.Add(EventType.ReceivedRelease,	ReceivedRelease);
			state.Add(EventType.ReceivedReleaseComplete,
				ReceivedReleaseComplete);
			state.Add(EventType.CallInfo); //,			DoNothing,	requestHoldAfterFirstCallInfo);
			state.Add(EventType.Resume,				Resume);//,		requestResume);
			
			state.Add(EventType.MohDisabled);//,		DoNothing,	requestHoldMohDisabled);
		}

		# endregion

		#endregion

		/// <summary>
		/// Saves media info for subsequent inclusion in an OpenReceiveResponse
		/// message we send down to the stack in response to an
		/// OpenReceiveRequest.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SetMedia.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleSaveMedia(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			SetMediaEvent message = event_.EventMessage as SetMediaEvent;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected SetMediaEvent, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				this_.media = message.Media;
			}
		}

		/// <summary>
		/// Places a call and saves media info for inclusion in a subsequent
		/// OpenReceiveResponse we send down to the stack.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SetMedia.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandlePlaceCall(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			SetMediaEvent message = event_.EventMessage as SetMediaEvent;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected SetMediaEvent, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				this_.media = message.Media;
				this_.InitiateCallWithStack(this_.to, ref followupEvents);
			}
		}

		/// <summary>
		/// Places a call without media info. (P2P call.)
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandlePlaceCallNoMedia(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.media = null;
			this_.Send(new Setup(this_.to));
		}

		/// <summary>
		/// Responds to an earlier OpenReceiveRequest message from the stack
		/// now that media has been set.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SetMedia.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleSetMediaAndRespond(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			SetMediaEvent message = event_.EventMessage as SetMediaEvent;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected SetMediaEvent, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				this_.media = message.Media;

				// Since passthruPartyId is unique for each request/response pair,
				// (save so that we don't do any work after the send then)
				// indicate that it has been "used."
				uint tempPassthruPartyId = this_.passthruPartyId;
				this_.passthruPartyId = 0;

				this_.SendOpenReceiveResponse(this_.media, tempPassthruPartyId);
			}
		}

		/// <summary>
		/// Responds to an earlier OpenReceiveRequest message from the stack
		/// now that media has been set.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SetMedia.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleSetMediaAndRespondAndCheckMoh(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			SetMediaEvent message = event_.EventMessage as SetMediaEvent;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected SetMediaEvent, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				this_.media = message.Media;

				// Since passthruPartyId is unique for each request/response pair,
				// (save so that we don't do any work after the send then)
				// indicate that it has been "used."
				uint tempPassthruPartyId = this_.passthruPartyId;
				this_.passthruPartyId = 0;

				this_.SendOpenReceiveResponse(this_.media, tempPassthruPartyId);

				if (!this_.provider.IsMohEnabled)
					this_.device.CallTrigger((int)EventType.MohDisabled);
			}
		}

		/// <summary>
		/// Sends SCCP message to CallManager if relationship still exists.
		/// </summary>
		/// <param name="message">SCCP message.</param>
		protected void Send(SccpMessage message)
		{
			if (device != null)
			{
				try
				{
					device.Send(message);
				}
				catch (NullReferenceException)
				{
					// If the components become null after checking, just ignore exception.
				}
			}
		}

		/// <summary>
		/// Sends internal message to SCCP stack if relationship still exists.
		/// </summary>
		/// <param name="message">Internal message.</param>
		protected void Send(ClientMessage message)
		{
			if (device != null)
			{
				try
				{
					device.Send(message);
				}
				catch (NullReferenceException)
				{
					// If the components become null after checking, just ignore exception.
				}
			}
		}

		/// <summary>
		/// Returns whether the endpoints are different and "real"--not the
		/// bogus address we use as temporary media destination. The bogus
		/// address is essentially a wild-card--any other address matches it.
		/// </summary>
		/// <param name="endpoint1">An IPEndPoint.</param>
		/// <param name="endpoint2">Another IPEndPoint.</param>
		private bool DifferentRealMediaAddresses(IPEndPoint endpoint1,
			IPEndPoint endpoint2)
		{
			bool different;

			different = !endpoint1.Equals(endpoint2) &&
				!endpoint1.Equals(provider.BitBucket) &&
				!endpoint2.Equals(provider.BitBucket);

			LogVerbose("Prv: addresses {0} and {1} are {2} (BitBucket: {3})",
				endpoint1, endpoint2,
				different ? "different" : "the same", provider.BitBucket);

			return different;
		}

		/// <summary>
		/// Resumes Call.
		/// </summary>
		/// <remarks>
		/// Telephony Manager is taking call off hold (resuming) by setting
		/// media info.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SetMedia.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleResume(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			SetMediaEvent message = event_.EventMessage as SetMediaEvent;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected SetMediaEvent, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				this_.media = message.Media;

				this_.Send(new FeatureRequest(FeatureRequest.Feature.Resume));
			}
		}

		/// <summary>
		/// Clears media info so that we won't use it in a subsequent
		/// OpenReceiveResponse--The Telephony Manager must invoke SetMedia
		/// again, which sets the media info, before we can respond to an
		/// OpenReceiveRequest.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleClearMedia(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.media = null;
		}

		/// <summary>
		/// Places call on hold.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleHold(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.media = null;
			this_.Send(new FeatureRequest(FeatureRequest.Feature.Hold));
		}

		/// <summary>
		/// Place call on hold and send MediaChanged message to TM.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleHoldAndSendMediaChanged(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.media = null;
			this_.Send(new FeatureRequest(FeatureRequest.Feature.Hold));
			this_.provider.MediaChanged(this_.CallId, null, 0); 
		}
		/// <summary>
		/// Starts the "hold tone" by simply setting a local variable to true.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleStartHoldTone(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
		}

		/// <summary>
		/// Stop the "hold tone" by simply setting a local variable to false.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleStopHoldTone(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
		}

		/// <summary>
		/// Handles the receipt of OpenReceiveRequest by saving the
		/// passthruPartyId (for when we subsequently respond with
		/// OpenReceiveResponse), and sending the GotCapabilities event up to
		/// the Telephony Manager.
		/// </summary>
		/// <remarks>
		/// We cannot respond to the OpenReceiveRequest message now because we
		/// don't know what address to include for where to send media.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for OpenReceiveRequest.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandlePendingOpenReceiveRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			OpenReceiveRequest message =
				event_.EventMessage as OpenReceiveRequest;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected OpenReceiveRequest, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				if (this_.passthruPartyId != 0)
				{
					this_.log.Write(TraceLevel.Warning,
						"Prv: {0}: possible duplicate OpenReceiveRequest",
						this_);
				}

				// Save for inclusion in subsequent OpenReceiveResponse.
				this_.passthruPartyId = message.media.passthruPartyId;

				this_.provider.GotCapabilities(this_.callId, null);
			}
		}

		/// <summary>
		/// Saves PassthroughId
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleSavePassthroughId(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			OpenReceiveRequest message =
				event_.EventMessage as OpenReceiveRequest;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected OpenReceiveRequest, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				if (this_.passthruPartyId != 0)
				{
					this_.log.Write(TraceLevel.Warning,
						"Prv: {0}: possible duplicate OpenReceiveRequest",
						this_);
				}

				// Save for inclusion in subsequent OpenReceiveResponse.
				this_.passthruPartyId = message.media.passthruPartyId;
			}
		}
		/// <summary>
		/// Responds to this OpenReceiveRequest because we already know what
		/// address to include for where to send media.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for OpenReceiveRequest.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleRespondToOpenReceiveRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.PleaseRespondToOpenReceiveRequest(event_, this_.media);
		}

		/// <summary>
		/// Responds to this OpenReceiveRequest because we already know what
		/// address to include for where to send media.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for OpenReceiveRequest.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleRespondToOpenReceiveRequestAndCheckMoh(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.PleaseRespondToOpenReceiveRequest(event_, this_.media);

			if (!this_.provider.IsMohEnabled)
				this_.device.CallTrigger((int)EventType.MohDisabled);
		}


	
		/// <summary>
		/// Responds to this OpenReceiveRequest with a dummy destination
		/// address.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for OpenReceiveRequest.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleRespondToOpenReceiveRequestWithDummy(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			this_.PleaseRespondToOpenReceiveRequest(event_, this_.GetDummyMediaInfo());
		}

		/// <summary>
		/// Sends MediaEstablished to Telephony Manager using info from
		/// StartTransmit.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for StartTransmit.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleEstablishMedia(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			StartTransmit message = event_.EventMessage as StartTransmit;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected StartTransmit, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				IMediaControl.Codecs codec = ConvertCodec(message.media.payloadType);
				if (codec == IMediaControl.Codecs.Unspecified)
				{
					this_.log.Write(TraceLevel.Error,
						"Prv: {0}: received incomingCall with unrecognized codec: {1}; ignored",
						this_, message.media.payloadType);
				}
				else
				{
					string transmitIp = message.media.address.Address.ToString();
					uint transmitPort = (uint)message.media.address.Port;
					uint frameSize = message.media.packetSize;

					this_.provider.MediaEstablished(this_.callId, transmitIp,
						transmitPort, transmitIp, transmitPort + 1,
						codec, frameSize, codec, frameSize);
				}
			}
		}

		/// <summary>
		/// Sends MediaEstablished to Telephony Manager using info from
		/// StartTransmit.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for StartTransmit.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleEstablishMediaAndCheckMoh(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			StartTransmit message = event_.EventMessage as StartTransmit;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected StartTransmit, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				IMediaControl.Codecs codec = ConvertCodec(message.media.payloadType);
				if (codec == IMediaControl.Codecs.Unspecified)
				{
					this_.log.Write(TraceLevel.Error,
						"Prv: {0}: received incomingCall with unrecognized codec: {1}; ignored",
						this_, message.media.payloadType);
				}
				else
				{
					string transmitIp = message.media.address.Address.ToString();
					uint transmitPort = (uint)message.media.address.Port;
					uint frameSize = message.media.packetSize;

					this_.provider.MediaEstablished(this_.callId, transmitIp,
						transmitPort, transmitIp, transmitPort + 1,
						codec, frameSize, codec, frameSize);

					if (!this_.provider.IsMohEnabled)
						this_.device.CallTrigger((int)EventType.MohDisabled);
				}
			}
		}

		[Action()]
		private static void HandleEstablishMediaAndSendCallEstablished(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			StartTransmit message = event_.EventMessage as StartTransmit;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected StartTransmit, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				IMediaControl.Codecs codec = ConvertCodec(message.media.payloadType);
				if (codec == IMediaControl.Codecs.Unspecified)
				{
					this_.log.Write(TraceLevel.Error,
						"Prv: {0}: received incomingCall with unrecognized codec: {1}; ignored",
						this_, message.media.payloadType);
				}
				else
				{
					string transmitIp = message.media.address.Address.ToString();
					uint transmitPort = (uint)message.media.address.Port;
					uint frameSize = message.media.packetSize;

					this_.provider.MediaEstablished(this_.callId, transmitIp,
						transmitPort, transmitIp, transmitPort + 1,
						codec, frameSize, codec, frameSize);

					this_.provider.CallEstablished(this_.callId, this_.to, this_.from);
				}
			}
		}

		/// <summary>
		/// Notifies Telephony Manager that media has changed whenever we
		/// receive a StartTransmit message, now that media has been
		/// established.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SetMedia.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleChangeMedia(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			StartTransmit message = event_.EventMessage as StartTransmit;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected StartTransmit, got {1}; ignored",
					this_, event_.EventMessage);
			}
			else
			{
				IMediaControl.Codecs codec = ConvertCodec(message.media.payloadType);
				if (codec == IMediaControl.Codecs.Unspecified)
				{
					this_.log.Write(TraceLevel.Error,
						"Prv: {0}: received incomingCall with unrecognized codec: {1}; ignored",
						this_, message.media.payloadType);
				}
				else
				{
					string transmitIp =
						message.media.address.Address.ToString();
					uint transmitPort = (uint)message.media.address.Port;
					uint frameSize = message.media.packetSize;

					this_.provider.MediaChanged(this_.callId, transmitIp,
						transmitPort, transmitIp, transmitPort + 1,
						codec, frameSize, codec, frameSize);
				}
			}
		}

		/// <summary>
		/// Notifies Telephony Manager that media has been cleared--IOW, stop
		/// transmitting--whenever we receive a StopTransmit message before
		/// media has been established.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleEstablishNoMedia(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			// (Have to pass empty string rather than null to indicate "no IP
			// address.")
			this_.provider.MediaEstablished(this_.callId, string.Empty, 0, string.Empty, 0,
				IMediaControl.Codecs.Unspecified, 0,
				IMediaControl.Codecs.Unspecified, 0);
		}

		/// <summary>
		/// Clears media object and notifies Telephony Manager that media has
		/// been cleared--IOW, stop transmitting--whenever we receive a
		/// StopTransmit message, now that media has been established, 
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleChangeToNoMedia(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.media = null;

			// (Have to pass empty string rather than null to indicate "no IP
			// address.")
			this_.provider.MediaChanged(this_.callId, string.Empty, 0);
		}

		/// <summary>
		/// Handles the Accept event from the Telephony Manager by sending
		/// SetupAck, Proceeding, and Alerting all at once to the stack.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleAccept(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(new SetupAck());
			this_.Send(new Proceeding());
			this_.Send(new Alerting());
		}

		/// <summary>
		/// Handles the Answer event from the Telephony Manager by sending
		/// Connect to the stack.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleAnswer(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(new Connect());
		}

		/// <summary>
		/// Handles the Redirect event from the Telephony Manager.
		/// </summary>
		/// <remarks>
		/// Since an SCCP client cannot redirect a call, we simple terminate
		/// it.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleRedirect(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.log.Write(TraceLevel.Warning,
				"Prv: {0}: SCCP does not support redirect; ignored",
				this_);

			this_.Send(new ReleaseComplete());

			this_.device.RemoveCall();
		}

		/// <summary>
		/// Handles the Reject event from the Telephony Manager.
		/// </summary>
		/// <remarks>
		/// Since an SCCP client cannot reject a call (only a CallManager can),
		/// we simple terminate it.
		/// </remarks>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReject(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.log.Write(TraceLevel.Warning,
				"Prv: {0}: SCCP does not support reject; ignored",
				this_);

			this_.Send(new ReleaseComplete());

			this_.device.RemoveCall();
		}

		/// <summary>
		/// Handles the SendUserInput event from the Telephony Manager by
		/// sending the specified DTMF digits.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Digits.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleSendUserInput(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(event_.EventMessage as Digits);
		}

		/// <summary>
		/// Handles Hangup event from Telephony Manager by releasing the call.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleHangup(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(new Release());
			//For p2p call with early media, HandleHangup is called twice.
			//So I have to add this null check to prevent RemoveCall from
			//called twice.
			if (this_.device != null)	
				this_.device.RemoveCall();
		}

		/// <summary>
		/// Handles the Connect event from the stack by responding with
		/// ConnectAck and sending an event up to the Telephony Manager
		/// reporting that the call is now established.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Connect.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReceivedConnect(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			Connect message = event_.EventMessage as Connect;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected Connect, got {1}; ignored", this_,
					event_.EventMessage);
			}
			else
			{
				this_.Send(new ConnectAck());

				this_.provider.CallEstablished(this_.callId, this_.to, this_.from);
			}
		}

		[Action()]
		private static void HandleSendConnectAck(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			Connect message = event_.EventMessage as Connect;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected Connect, got {1}; ignored", this_,
					event_.EventMessage);
			}
			else
			{
				this_.Send(new ConnectAck());
			}
		}

		/// <summary>
		/// Handles the ConnectAck event from the stack by sending an event
		/// up to the Telephony Manager reporting that the call is now
		/// established.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReceivedConnectAck(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.provider.CallEstablished(this_.callId, this_.to, this_.from);
		}

		/// <summary>
		/// Handles the Release event from the stack by responding with
		/// ReleaseComplete and sending a Hangup event up to the Telephony
		/// Manager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReceivedRelease(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(new ReleaseComplete());

			this_.provider.Hangup(this_.callId);
            if (this_.device != null)
			    this_.device.RemoveCall();
		}

		/// <summary>
		/// Handles the Release event from the stack with the possibility of
		/// failing over to a device in another device pool. Respond with
		/// ReleaseComplete and then, if failover, attempt to place a call on
		/// another device so TM thinks we are still working on the same call
		/// it initiated; otherwise, send a Hangup event up to the Telephony
		/// Manager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReceivedReleaseConsiderFailover(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.Send(new ReleaseComplete());

			if (!this_.Failover(ReleaseComplete.Cause.Normal, ref followupEvents))
			{
				this_.provider.Hangup(this_.callId);
                if (this_.device != null)
				    this_.device.RemoveCall();
			}
		}

		/// <summary>
		/// Handles the ReleaseComplete event from the stack by sending a
		/// Hangup event up to the Telephony Manager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReceivedReleaseComplete(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;

			this_.provider.Hangup(this_.callId);
            if (this_.device != null)
		        this_.device.RemoveCall();
		}

		/// <summary>
		/// Handles the ReleaseComplete event from the stack with the
		/// possibility of failing over to a device in another device pool.
		/// If failover, attempt to place a call on another device so TM thinks
		/// we are still working on the same call it initiated; otherwise,
		/// send a Hangup event up to the Telephony Manager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">ReleaseComplete message containing cause.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReceivedReleaseCompleteConsiderFailover(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			ReleaseComplete message = event_.EventMessage as ReleaseComplete;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected ReleaseComplete, got {1}; ignored", this_,
					event_.EventMessage);
			}
			else
			{
				if (!this_.Failover(message.cause, ref followupEvents))
				{
					this_.provider.Hangup(this_.callId);
					this_.device.RemoveCall();
				}
			}
		}

		/// <summary>
		/// Handles the Digits event from the stack by sending the GotDigits
		/// event up to the Telephony Manager containing the DTMF digits.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for Digits.</param>
		/// <param name="followupEvents">No followup events--ignored.</param>
		[Action()]
		private static void HandleReceivedDigits(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Call this_ = stateMachine as Call;
			Digits message = event_.EventMessage as Digits;
			if (message == null)
			{
				this_.log.Write(TraceLevel.Error,
					"Prv: {0}: expected Digits, got {1}; ignored", this_,
					event_.EventMessage);
			}
			else
			{
				this_.provider.GotDigits(this_.callId, message.digits);
			}
		}

		[Action()]
		private static void HandleDoNothing(StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
		}

		#region Support methods to the HandleX methods

		/// <summary>
		/// Responds to an OpenReceiveRequest.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for OpenReceiveRequest.</param>
		/// <returns>null--no followup events.</returns>
		private ArrayList PleaseRespondToOpenReceiveRequest(Event event_,
			MediaInfo mediaInfo)
		{
			OpenReceiveRequest message =
				event_.EventMessage as OpenReceiveRequest;
			if (message == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: expected OpenReceiveRequest, got {1}; ignored",
					this, event_.EventMessage);
			}
			else
			{
				// We're not going to use this.passthruPartyId, but take a look
				// at it anyway. If non-zero, we must be superceding a pending
				// OpenReceiveRequest, which should never ever happen.
				if (passthruPartyId != 0)
				{
					log.Write(TraceLevel.Warning,
						"Prv: {0}: possible duplicate OpenReceiveRequest",
						this);
					passthruPartyId = 0;
				}

				SendOpenReceiveResponse(mediaInfo,
					message.media.passthruPartyId);
			}

			return null;
		}

		/// <summary>
		/// Returns media with a bogus address yet hopefully acceptable to the
		/// CallManager.
		/// </summary>
		/// <returns>Media containing bogus address.</returns>
		private MediaInfo GetDummyMediaInfo()
		{
			return new MediaInfo(provider.BitBucket, (uint)callId, 0, 20,
				PayloadType.G711Ulaw64k, false, 184, false, 0);
		}


		/// <summary>
		/// Sends an OpenReceiveResponse back down to the stack presumably in
		/// response to a previous OpenReceiveRequest from the stack.
		/// </summary>
		/// <param name="mediaInfo">Media-capability info to include in the
		/// OpenReceiveResponse message.</param>
		/// <param name="passthruPartyId">Pass-through party id (transaction
		/// identifier) from previous OpenReceiveRequest.</param>
		private void SendOpenReceiveResponse(MediaInfo mediaInfo,
			uint passthruPartyId)
		{
			if (mediaInfo != null)
			{
				if (passthruPartyId != 0)
				{
					// Be sure to hairpin the passThruParty ID or all hell
					// breaks loose.
					mediaInfo.passthruPartyId = passthruPartyId;

					LogVerbose("Prv: {0}: sending OpenReceiveResponse with address {1}",
						this, mediaInfo.address);

					Send(new OpenReceiveResponse(mediaInfo));
				}
				else
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: attempt to send OpenReceiveResponse without passthruPartyId",
						this);
				}
			}
			else
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: attempt to send OpenReceiveResponse without media",
					this);
			}
		}

		/// <summary>
		/// Converts a common payload type used within the Telephony Manager
		/// to the corresponding payload type used within the SCCP stack.
		/// </summary>
		/// <param name="codec">Common payload type used within the Telephony
		/// Manager.</param>
		/// <returns>Payload type used within the SCCP stack.</returns>
		private static PayloadType ConvertCodec(IMediaControl.Codecs codec)
		{
			PayloadType payloadType;

			switch (codec)
			{
				case IMediaControl.Codecs.G711u:
				default:
					payloadType = PayloadType.G711Ulaw64k;
					break;
				case IMediaControl.Codecs.G711a:
					payloadType = PayloadType.G711Alaw64k;
					break;
				case IMediaControl.Codecs.G729:
					payloadType = PayloadType.G729AnnexA;
					break;
			}

			return payloadType;
		}

		/// <summary>
		/// Converts a payload type used within the SCCP stack to the
		/// corresponding common payload type used within the Telephony
		/// Manager.
		/// </summary>
		/// <param name="codec">Payload type used within the SCCP
		/// stack.</param>
		/// <returns>Common payload type used within the Telephony
		/// Manager.</returns>
		private static IMediaControl.Codecs ConvertCodec(PayloadType payload)
		{
			IMediaControl.Codecs codec;

			switch(payload)
			{
				case PayloadType.G711Ulaw64k:
					codec = IMediaControl.Codecs.G711u;
					break;
				case PayloadType.G711Alaw64k:
					codec = IMediaControl.Codecs.G711a;
					break;
				case PayloadType.G729AnnexA:
					codec = IMediaControl.Codecs.G729;
					break;
				default:
					codec = IMediaControl.Codecs.Unspecified;
					break;
			}

			return codec;
		}

		/// <summary>
		/// Fails-over this Call to a Device in another device pool if
		/// necessary and possible.
		/// </summary>
		/// <remarks>Currently only implemented by OutgoingCall.</remarks>
		/// <param name="cause">Cause of ReleaseComplete.</param>
		/// <param name="followupEvents">Followup events.</param>
		/// <returns>Whether the failover was successful.</returns>
		protected virtual bool Failover(ReleaseComplete.Cause cause,
			ref Queue followupEvents)
		{
			return false;
		}

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
		/// Logs a State Transition.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event triggering the Transition.</param>
		/// <param name="nextState">The State to which we are
		/// transitioning.</param>
		protected override void LogTransition(Event event_, State nextState)
		{
			LogVerbose("Prv: {0}: {1}: {2} -> {3}",
				this, IntToEventEnumString(event_.Id), CurrentState,
				nextState);
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
		protected void LogVerbose(string text)
		{
			if (provider.IsLogCallVerbose)
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
		protected void LogVerbose(string text, params object[] args)
		{
			if (provider.IsLogCallVerbose)
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
			return callId.ToString();
		}
	}

	#region Call constructors

	/// <summary>
	/// Represents an incoming SCCP call.
	/// </summary>
	/// <remarks>
	/// Doesn't do much for now--provided for completeness with the outgoing
	/// Call constructors. Might be handy down the road.
	/// </remarks>
	public class IncomingCall : Call
	{
		/// <summary>
		/// Constructs an IncomingCall.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		/// <param name="displayName">Display name of caller.</param>
		public IncomingCall(SccpProvider provider, LogWriter log, long callId,
			string to, string from, string originalTo, string displayName) :
			base(provider, log, callId, to, from, originalTo, displayName)
		{
			LogVerbose("Prv: {0}: created incoming call", this);
		}

		/// <summary>
		/// Property whose value is whether this call expects to receive a
		/// Setup message from the stack during the call.
		/// </summary>
		/// <remarks>
		/// An IncomingCall never expects to receive a Setup during the call.
		/// </remarks>
		public override bool SetupExpectedDuringCall { get { return false; } }

		/// <summary>
		/// Do nothing since this "can't happen."
		/// </summary>
		/// <param name="to">Ignored.</param>
		/// <param name="followupEvents">Ignored.</param>
		protected override void InitiateCallWithStack(string to, ref Queue followupEvents)
		{
			Debug.Fail("SccpStack: attempt to initiate an IncomingCall");
		}
	}

	/// <summary>
	/// Represents a normal outgoing SCCP call.
	/// </summary>
	/// <remarks>
	/// This class provides slightly different behavior for a normal outgoing
	/// call versus a barged call.
	/// </remarks>
	public class OutgoingCall : Call
	{
		/// <summary>
		/// Constructs an OutgoingCall.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		/// <param name="outCallInfo">Call metadata from app/TM action.</param>
		public OutgoingCall(SccpProvider provider, LogWriter log,
			OutboundCallInfo outCallInfo) :
			base(provider, log, outCallInfo.CallId, outCallInfo.To,
			outCallInfo.From, outCallInfo.To)
		{
			LogVerbose("Prv: {0}: created outgoing call", this);

			// Save in case need to failover to another device.
			this.outCallInfo = outCallInfo;
		}

		/// <summary>
		/// Call metadata from app/TM action.
		/// </summary>
		internal OutboundCallInfo outCallInfo;

		/// <summary>
		/// Property whose value is whether this call expects to receive a
		/// Setup message from the stack during the call.
		/// </summary>
		/// <remarks>
		/// An OutgoingCall never expects to receive a Setup during the call.
		/// </remarks>
		public override bool SetupExpectedDuringCall { get { return false; } }

		/// <summary>
		/// Send a Setup message to the stack to initiate a normal outgoing
		/// call.
		/// </summary>
		/// <param name="to">Directory number of the party we are
		/// calling.</param>
		/// <param name="followupEvents">Ignored.</param>
		protected override void InitiateCallWithStack(string to, ref Queue followupEvents)
		{
			Send(new Setup(to));
		}

		/// <summary>
		/// Fails-over this Call to a Device in another device pool if
		/// necessary and possible.
		/// </summary>
		/// <param name="cause">Cause of ReleaseComplete.</param>
		/// <param name="followupEvents">Followup events.</param>
		/// <returns>Whether the failover was successful.</returns>
		protected override bool Failover(ReleaseComplete.Cause cause,
			ref Queue followupEvents)
		{
			bool failedover = false;

			switch (cause)
			{
				// E.g., the remote party hung up.
				case ReleaseComplete.Cause.Normal:
				// Can't happen, but just in case.
				case ReleaseComplete.Cause.NoCallOnLine:
					break;

				default:
					bool nextDevicePool;
					switch (cause)
					{
						// Rare. CallId value would have had to wrap around.
						case ReleaseComplete.Cause.CallIdInUse:
							nextDevicePool = false;
							break;

						// Assuming that if one device is not connected to or
						// registered with CallManager then none of them are so
						// maybe better luck with the next device pool.
						case ReleaseComplete.Cause.NotConnected:
						case ReleaseComplete.Cause.NotRegistered:
						// Can't happen.
						default:
							nextDevicePool = true;
							break;
					}

					// Get name of next available device in Call Route Group so
					// that we can failover to it.
					string toMacAddress = provider.GetDeviceNameFromRouteGroup(
						outCallInfo, nextDevicePool);
					if (toMacAddress == null)
					{
						log.Write(TraceLevel.Warning,
							"Prv: {0}: all devices associated with {1}:{2} are in use or not available; cannot failover call",
							this, outCallInfo.AppName, outCallInfo.PartitionName);
					}
					else
					{
						// Move this Call from the current Device with which it is
						// associated to the new one to which we are failing over.
						if (!provider.MoveCall(Device.MacAddress, toMacAddress))
						{
							log.Write(TraceLevel.Warning,
								"Prv: {0}: cannot move call to another device; cannot failover call",
								this);
						}
						else
						{
							InitiateCallWithStack(outCallInfo.To, ref followupEvents);

							failedover = true;
						}
					}
					break;
			}

			return failedover;
		}
	}

	/// <summary>
	/// Represents a barged outgoing SCCP call.
	/// </summary>
	/// <remarks>
	/// This class provides slightly different behavior for a barged call
	/// versus a normal outgoing call.
	/// </remarks>
	public class BargedCall : Call
	{
		/// <summary>
		/// Constructs a BargedCall.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		/// <param name="lastCallRemoteMultilineCallId">Call id from the most
		/// recent CallState(CallRemoteMultiline) before this BargedCall was
		/// constructed.</param>
		public BargedCall(SccpProvider provider, LogWriter log, long callId,
			string to, string from, string originalTo,
			uint lastCallRemoteMultilineCallId) :
			base(provider, log, callId, to, from, originalTo)
		{
			this.lastCallRemoteMultilineCallId = lastCallRemoteMultilineCallId;

			LogVerbose("Prv: {0}: created barged call", this);
		}

		/// <summary>
		/// Call id (different from the TM callId) from the most recent
		/// CallState(CallRemoteMultiline). Barge needs it.
		/// </summary>
		private uint lastCallRemoteMultilineCallId;

		/// <summary>
		/// Property whose value is whether this call expects to receive a
		/// Setup message from the stack during the call.
		/// </summary>
		/// <remarks>
		/// A BargedCall always expects to receive a Setup during the call.
		/// I don't know why it is doing it--that's just an oddity of the
		/// stack.
		/// </remarks>
		public override bool SetupExpectedDuringCall { get { return true; } }

		/// <summary>
		/// Send a SoftkeyEvent of type, Barge, through the stack to the
		/// CallManager to initiate a barged call.
		/// </summary>
		/// <param name="to">Ignored since we're barging on a line and not
		/// "dialing" a number.</param>
		/// <param name="followupEvents">Followup events.</param>
		protected override void InitiateCallWithStack(string to, ref Queue followupEvents)
		{
			Send(new SoftkeyEvent(SoftkeyEventType.Barge, 1,
				lastCallRemoteMultilineCallId));

			// We would normally end up in the MediaSetOutgoingCallPending state,
			// but we don't want to be "pending"--wait to detect early call
			// failure--so proceed to the next state. This is because we want
			// to barge into a particular call, so failing over to another
			// Device doesn't make sense.
			followupEvents.Enqueue(new Event((int)EventType.ProceedFromPending, this));
		}
	}

	#endregion

	/// <summary>
	/// Represents a SetMedia action from the Telephony Manager.
	/// </summary>
	/// <remarks>
	/// This is the encapsulation mechanism of the state machine that we're
	/// using. If we want to include data along with an event id, we have to
	/// wrap it up in an SccpStack.Message.
	/// </remarks>
	public class SetMediaEvent : Message
	{
		/// <summary>
		/// Constructs a SetMediaEvent.
		/// </summary>
		/// <remarks>
		/// Only the address and passthruPartyId fields in the
		/// OpenReceiveResponse message are used by the stack. Kind of a waste,
		/// isn't it?
		/// </remarks>
		/// <param name="mediaAddr">IPEndPoint address.</param>
		public SetMediaEvent(IPEndPoint mediaAddr)
		{
			media = new MediaInfo(mediaAddr, 0, 0, 0, PayloadType.G711Ulaw64k,
				false, 0, false, 0);
		}

		/// <summary>
		/// Media-capability information.
		/// </summary>
		private MediaInfo media;

		/// <summary>
		/// Property whose value is the media-capability information.
		/// </summary>
		public MediaInfo Media { get { return media; } }
	}
}
