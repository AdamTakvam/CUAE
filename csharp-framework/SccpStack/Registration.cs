using System;
using System.Diagnostics;
using System.Collections;
using System.Net;

using Metreos.LoggingFramework;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents the state machine that registers the device with a
	/// CallManager.
	/// </summary>
	/// <remarks>
	/// Was called sccpreg in the PSCCP code.
	/// </remarks>
	[StateMachine(ActionPrefix="Handle", StateDefinitionPrefix="Define")]
	internal class Registration : SccpStateMachine
	{
		/// <summary>
		/// Constructs Registration object.
		/// </summary>
		/// <param name="device">Device on which this object is being
		/// used.</param>
		/// <param name="log">Object through which log entries are generated.</param>
		internal Registration(Device device, LogWriter log) :
			base("Reg", device, log, ref control)
		{
			Initialize();
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
		/// new Registration();
		/// </example>
		/// <param name="log">Object through which log entries are generated.</param>
		public Registration(LogWriter log) : base(log, ref control) { }

		/// <summary>
		/// Object through which StateMachine assures that static state machine
		/// is constructed only once.
		/// </summary>
		private static StateMachineStaticControl control = new StateMachineStaticControl();

		/// <summary>
		/// Resets all member variables to their original values.
		/// </summary>
		private void Initialize()
		{
			LogVerbose("Reg: {0}: initializing", this);
			connection = null;
			miscCount = 0;

			deviceData = new GapiStatusDataInfoType();

			tempLineCount = 0;
			tempSpeeddialCount = 0;
			tempFeatureCount = 0;
			tempServiceUrlCount = 0;
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
		/// Maximum number of lines.
		/// </summary>
		/// <remarks>
		/// This came from the original PSCCP code. I have no idea where
		/// the particular value comes from, though.
		/// </remarks>
		private static volatile int maxLines = 42;

		/// <summary>
		/// Maximum number of lines.
		/// </summary>
		internal static int MaxLines { set { maxLines = value; } }

		/// <summary>
		/// Maximum number of RTP streams per device.
		/// </summary>
		internal static int MaxRtpStreams { get { return maxLines; } }

		/// <summary>
		/// Maximum number of features per device.
		/// </summary>
		private static volatile int maxFeatures = 42;

		/// <summary>
		/// Maximum number of features per device.
		/// </summary>
		internal static int MaxFeatures { set { maxFeatures = value; } }

		/// <summary>
		/// Maximum number of service URLs per device.
		/// </summary>
		private static volatile int maxServiceUrls = 42;

		/// <summary>
		/// Maximum number of service URLs per device.
		/// </summary>
		internal static int MaxServiceUrls { set { maxServiceUrls = value; } }

		/// <summary>
		/// Maximum number of Softkey definitions per device.
		/// </summary>
		private static volatile int maxSoftkeyDefinitions = 42;

		/// <summary>
		/// Maximum number of Softkey definitions per device.
		/// </summary>
		internal static int MaxSoftkeyDefinitions { set { maxSoftkeyDefinitions = value; } }

		/// <summary>
		/// Maximum number of SoftkeySet definitions per device.
		/// </summary>
		private static volatile int maxSoftkeySetDefinitions = 42;

		/// <summary>
		/// Maximum number of SoftkeySet definitions per device.
		/// </summary>
		internal static int MaxSoftkeySetDefinitions { set { maxSoftkeySetDefinitions = value; } }

		/// <summary>
		/// Maximum number of speeddials per device.
		/// </summary>
		private static volatile int maxSpeeddials = 42;

		/// <summary>
		/// Maximum number of speeddials per device.
		/// </summary>
		internal static int MaxSpeeddials { set { maxSpeeddials = value; } }

		/// <summary>
		/// Codecs to register with the CallManager.
		/// </summary>
		private static ArrayList mediaCapabilities = new ArrayList();

		/// <summary>
		/// Codecs to register with the CallManager.
		/// </summary>
		internal static ArrayList Codecs { get { return mediaCapabilities; } }
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
		/// Connection to CallManager.
		/// </summary>
		private SccpConnection connection;

		/// <summary>
		/// Count of miscellaneous stimili.
		/// </summary>
		private volatile int miscCount;

		/// <summary>
		/// A bunch of device-related data.
		/// </summary>
		/// <remarks>
		/// I'm not real sure why these particular datum are grouped together
		/// here. This was simply ported from the original PSCCP code.
		/// </remarks>
		private GapiStatusDataInfoType deviceData;

		/// <summary>
		/// Return clone of deviceData.
		/// </summary>
		/// <returns>Device data.</returns>
		internal GapiStatusDataInfoType GetDeviceData()
		{
			return new GapiStatusDataInfoType(
				deviceData.LineCount, deviceData.SpeeddialCount,
				deviceData.FeatureCount, deviceData.ServiceUrlCount,
				deviceData.SoftkeyCount, deviceData.SoftkeySetCount,
				deviceData.Version, null);
		}

		/// <summary>
		/// Number of LineStateReq messages that this device has sent to the
		/// CallManager.
		/// </summary>
		private volatile int tempLineCount;

		/// <summary>
		/// Number of SpeeddialStateReq messages that this device has sent to
		/// the CallManager.
		/// </summary>
		private volatile int tempSpeeddialCount;

		/// <summary>
		/// Number of FeatureStatReq messages that this device has sent to the
		/// CallManager.
		/// </summary>
		private volatile int tempFeatureCount;

		/// <summary>
		/// Number of ServiceUrlStatReq messages that this device has sent to
		/// the CallManager.
		/// </summary>
		private volatile int tempServiceUrlCount;

		#region Finite State Machine

		#region State declarations
		// (No access control for states because once constructed, they are not changed.)
		private static State idle = null;
		private static State registering = null;
		private static State registered = null;
		private static State unregistering = null;
		#endregion

		/// <summary>
		/// Types of events that can trigger actions within this state machine.
		/// </summary>
		internal enum EventType
		{
			RegisterRequest,
			RegisterAck,
			RegisterReject,
			CapabilitiesRequest,
			ButtonTemplateResponse,
			SoftkeyTemplateResponse,
			SoftkeySetResponse,
			SelectSoftkeys,
			ConfigStatResponse,
			LineStatResponse,
			SpeeddialStatResponse,
			ForwardStatResponse,
			FeatureStatResponse,
			ServiceUrlStatResponse,
			VersionResponse,
			ServerResponse,
			UnregisterRequest,
			UnregisterAck,
			Registered,
			Cleanup,
		}

		#region Action declarations
		private static ActionDelegate Cleanup = null;
		private static ActionDelegate XxxRegisterRequest = null;
		private static ActionDelegate RegisterRequest = null;
		private static ActionDelegate RegisterAck = null;
		private static ActionDelegate RegisterReject = null;
		private static ActionDelegate CapabilitiesRequest = null;
		private static ActionDelegate ButtonTemplateResponse = null;
		private static ActionDelegate SoftkeyTemplateResponse = null;
		private static ActionDelegate SoftkeySetResponse = null;
		private static ActionDelegate LineStatResponse = null;
		private static ActionDelegate SpeeddialStatResponse = null;
		private static ActionDelegate FeatureStatResponse = null;
		private static ActionDelegate ServiceUrlStatResponse = null;
		private static ActionDelegate UnregisterRequest = null;
		private static ActionDelegate UnregisterAck_ = null;
		#endregion

		#region State-definition methods
		/// <summary>
		/// Defines the idle state where the Registration object has just been
		/// constructed and is ready for a RegisterRequest event to start the
		/// registration process with a CallManager.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State(Initial=true)]
		private static void DefineIdle(State state)
		{
			//					Event					Action						Next State
			state.Add(EventType.RegisterRequest,		RegisterRequest,			registering);
			state.Add(EventType.Cleanup,				Cleanup);
			state.Add(EventType.UnregisterAck);
		}

		/// <summary>
		/// Defines the registering state where we have typically sent the
		/// Register message to the CallManager and are waiting for the
		/// CallManager to respond.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineRegistering(State state)
		{
			//					Event					Action						Next State
			state.Add(EventType.RegisterRequest,		XxxRegisterRequest);
			state.Add(EventType.RegisterAck,			RegisterAck);
			state.Add(EventType.RegisterReject,			RegisterReject);
			state.Add(EventType.CapabilitiesRequest,	CapabilitiesRequest);
			state.Add(EventType.ButtonTemplateResponse,	ButtonTemplateResponse);
			state.Add(EventType.SoftkeyTemplateResponse,SoftkeyTemplateResponse);
			state.Add(EventType.SoftkeySetResponse,		SoftkeySetResponse);
			state.Add(EventType.LineStatResponse,		LineStatResponse);
			state.Add(EventType.SpeeddialStatResponse,	SpeeddialStatResponse);
			state.Add(EventType.FeatureStatResponse,	FeatureStatResponse);
			state.Add(EventType.ServiceUrlStatResponse,	ServiceUrlStatResponse);
			state.Add(EventType.UnregisterRequest,		UnregisterRequest,			unregistering);
			state.Add(EventType.Registered,											registered);
			state.Add(EventType.Cleanup,				Cleanup,					idle);
		}

		/// <summary>
		/// Defines the registered state which we typically occupy most of the
		/// time. The CallManager has accepted our request to register with it.
		/// </summary>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineRegistered(State state)
		{
			//					Event					Action						Next State
			state.Add(EventType.RegisterRequest,		XxxRegisterRequest);
			state.Add(EventType.ButtonTemplateResponse);		// We're going to restart anyway.
			state.Add(EventType.SpeeddialStatResponse);			// We're going to restart anyway.
			state.Add(EventType.FeatureStatResponse);			// We're going to restart anyway.
			state.Add(EventType.UnregisterRequest,		UnregisterRequest,			unregistering);
			state.Add(EventType.Registered);					// Can't see harm in recursive transition.
			state.Add(EventType.Cleanup,				Cleanup,					idle);
		}

		/// <summary>
		/// Defines the unregistering state where we have typically sent the
		/// Unregister message to the CallManager and are waiting for the
		/// CallManager to respond.
		/// </summary>
		/// <remarks>
		/// The stack often (always?) sends its own UnregisterAck event into
		/// this state machine so that it doesn't have to wait for the actual
		/// message from the CallManager. I don't know why it doesn't just wait
		/// for the real message.
		/// </remarks>
		/// <param name="state">Previously constructed state object.</param>
		[State()]
		private static void DefineUnregistering(State state)
		{
			//					Event					Action						Next State
			state.Add(EventType.RegisterRequest,		XxxRegisterRequest);
			state.Add(EventType.UnregisterAck,			UnregisterAck_);
			state.Add(EventType.Registered,											registered);
			state.Add(EventType.Cleanup,				Cleanup,					idle);
		}

		/// <summary>
		/// Cleans up Registration object.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCleanup(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;

			this_.Initialize();
		}

		/// <summary>
		/// Requests to register while in a non-idle state. Begins
		/// unregistration procedures and requeues the original RegisterRequest
		/// to be processed after the UnregisterRequest.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleXxxRegisterRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;

			this_.Send(new Unregister());

			followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));

			// Requeue the RegisterRequest
			followupEvents.Enqueue(new Event((int)EventType.RegisterRequest, this_));
		}

		/// <summary>
		/// Requests to register. Makes sure that there is a primary connection
		/// and begins registration.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegisterRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;

			// Make sure we are still connected to a CallManager.
			if (!this_.device.Discoverer.HavePrimaryConnection(out this_.connection))
			{
				CallManager callManager = this_.device.Discoverer.CallManager;
				// (Just in case there is no longer a CallManager object.)
				if (callManager == null)
				{
					this_.log.Write(TraceLevel.Warning,
						"Reg: {0}: CallManager object mysteriously disappeared; RegisterNak not sent to it", this_);
				}
				else
				{
					followupEvents.Enqueue(new Event((int)CallManager.EventType.RegisterNak,
						new RegisterReject(), callManager));
				}

				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
			else
			{
				this_.Send(new Alarm(Alarm.Severity.Informational,
					this_.device.Discoverer.CloseCauseToString(),
					Alarm.Param1Const.Composite, 0));

				this_.Send(new Register(
					new Sid("SEP" + this_.device.MacAddress),

					this_.device.Discoverer.ClientOverrideAddress != null ?
						this_.device.Discoverer.ClientOverrideAddress :
						this_.LocalEndPoint.Address,

					this_.device.Discoverer.DeviceType,
					(uint)MaxRtpStreams, 0, this_.device.Version, 0, 0));

				// (I guess pre-Seaview CallManagers expect an IpPort message
				// here.)
				if (this_.device.Version < ProtocolVersion.Seaview)
				{
					// (Don't know why always report port 0. This came from the
					// original PSCCP code.)
					this_.Send(new IpPort(0));
				}

				// Reset the TCP close cause. The value should be reset each
				// alarm cycle.
				this_.device.Discoverer.AlarmCondition =
					Discovery.Alarm.LastInitialized;
			}
		}

		/// <summary>
		/// Informs the CallManager object and proceeds with registration
		/// because our Registration request has been accepted by the
		/// CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for RegisterAck message.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegisterAck(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			RegisterAck message = event_.EventMessage as RegisterAck;

			// (Was using device.Discoverer.CallManager, but this method
			// seems to be safer, more direct. Don't really trust the old
			// method at all but decided to include it in case we can't
			// find CallManager the new way.)
			CallManager callManager;
			if (!this_.device.Discoverer.HaveCallManager(
				this_.RemoteEndPoint, out callManager))
			{
				callManager = this_.device.Discoverer.CallManager;
			}

			// (Just in case there is no longer a CallManager object.)
			if (callManager == null)
			{
				this_.log.Write(TraceLevel.Warning,
					"Reg: {0}: CallManager object mysteriously disappeared; RegisterAck not sent to it", this_);
			}
			else
			{
				followupEvents.Enqueue(new Event((int)CallManager.EventType.RegisterAck,
					message, callManager));
			}

			// (I guess post-Hawkbill CallManagers expect a HeadsetStatus
			// message here.)
			if (this_.device.Version > ProtocolVersion.Hawkbill)
			{
				this_.Send(new HeadsetStatus(false));
			}

			this_.Send(new ButtonTemplateReq());
		}

		/// <summary>
		/// Informs the CallManager object and cleans up because our
		/// Registration request has been rejected by the CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleRegisterReject(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			RegisterReject message = event_.EventMessage as RegisterReject;

			// (Was using device.Discoverer.CallManager, but this method
			// seems to be safer, more direct. Don't really trust the old
			// method at all but decided to include it in case we can't find
			// CallManager the new way.)
			CallManager callManager;
			if (!this_.device.Discoverer.HaveCallManager(this_.RemoteEndPoint,
				out callManager))
			{
				callManager = this_.device.Discoverer.CallManager;
			}

			// (Just in case there is no longer a CallManager object.)
			if (callManager == null)
			{
				this_.log.Write(TraceLevel.Warning,
					"Reg: {0}: CallManager object mysteriously disappeared; RegisterNak not sent to it", this_);
			}
			else
			{
				followupEvents.Enqueue(new Event((int)CallManager.EventType.RegisterNak,
					message, callManager));
			}

			followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
		}

		/// <summary>
		/// Returns capabilities to CallManager.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleCapabilitiesRequest(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;

			this_.Send(new CapabilitiesRes(mediaCapabilities));
		}

		/// <summary>
		/// Copies data from the ButtonTemplate message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for ButtonTemplate.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleButtonTemplateResponse(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			ButtonTemplate message = event_.EventMessage as ButtonTemplate;

			// Copy each button description.
			int count = message.buttons.Count;
			for (int i = (int)message.offset; count-- > 0 && i < message.total;
				++i)
			{
				// Translate the instance to internal. instance starts at 1,
				// but internally it is 0.
				int instance =
					((ButtonTemplate.Definition)message.buttons[i]).instance - 1;

				// The ButtonTemplate message includes both directory number
				// and speeddial values, so we need to look at each and copy
				// them to the right spot.
				// TBD - the statements that did this move were commented out
				// in the PSCCP. WTF?
				//
				// We also increment seperate counters for each type. These
				// counters are used to track if all button definitions
				// have been received and later to request info for each button
				// defined.
				switch ((DeviceStimulus)
					((ButtonTemplate.Definition)message.buttons[i]).definition)
				{
					case DeviceStimulus.Line:
						if (instance < maxLines)
						{
							this_.deviceData.LineCount++;
						}
						break;

					case DeviceStimulus.SpeedDial:
						if (instance < maxSpeeddials)
						{
							this_.deviceData.SpeeddialCount++;
						}
						break;

					case DeviceStimulus.Privacy:
						if (instance < maxFeatures)
						{
							this_.deviceData.FeatureCount++;
						}
						break;

					case DeviceStimulus.ServiceUrl:
						if (instance < maxServiceUrls)
						{
							this_.deviceData.ServiceUrlCount++;
						}
						break;

					default:
						this_.miscCount++;
						break;
				}
			}

			this_.LogVerbose(
				"Reg: {0}: lineCount: {1}, speeddialCount: {2}, " +
				"serviceUrlCount: {3}, featureCount: {4}, miscCount: {5}",
				this_, this_.deviceData.LineCount, this_.deviceData.SpeeddialCount,
				this_.deviceData.ServiceUrlCount, this_.deviceData.FeatureCount,
				this_.miscCount);

			// Request the softkey template only if we have received all button
			// definitions.
			if (this_.deviceData.LineCount +
				this_.deviceData.SpeeddialCount +
				this_.deviceData.ServiceUrlCount +
				this_.deviceData.FeatureCount + this_.miscCount == message.total)
			{
				this_.Send(new SoftkeyTemplateReq());
			}
		}

		/// <summary>
		/// Copies data from the SoftkeyTemplateRes message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SoftkeyTemplateRes.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSoftkeyTemplateResponse(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			SoftkeyTemplateRes message =
				event_.EventMessage as SoftkeyTemplateRes;

			this_.deviceData.SoftkeyCount +=
				message.softkeyTemplate.definitions.Count;

			int count = message.softkeyTemplate.definitions.Count;
			for (int i = (int)message.softkeyTemplate.offset; count-- > 0 &&
				// Make sure we still have space for the data.
				i < maxSoftkeyDefinitions &&
				i < message.softkeyTemplate.total; ++i)
			{
				// If already defined, remove from hash table.
				lock (this_.device.SappStatusDataInfo.Softkeys.SyncRoot)
				{
					this_.device.SappStatusDataInfo.Softkeys.Remove(i);

					// Add to hash table.
					this_.device.SappStatusDataInfo.Softkeys.Add(i,
						message.softkeyTemplate.definitions[i]);

					// Maintain count as if softkeys not a sparse array.
					if (i + 1 > this_.device.SappStatusDataInfo.SoftkeysCount)
					{
						this_.device.SappStatusDataInfo.SoftkeysCount = i + 1;
					}
				}
			}

			this_.LogVerbose("Reg: {0}: softkeyCount: {1}",
				this_, this_.deviceData.SoftkeyCount);

			// Request the softkey set only if we have received all softkey
			// templates.
			if (this_.deviceData.SoftkeyCount == message.softkeyTemplate.total)
			{
				this_.Send(new SoftkeySetReq());
			}
		}

		/// <summary>
		/// Copies data from the SoftkeySetRes message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SoftkeySetRes.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSoftkeySetResponse(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			SoftkeySetRes message = event_.EventMessage as SoftkeySetRes;

			this_.deviceData.SoftkeySetCount += message.set_.definitions.Count;

			int count = message.set_.definitions.Count;
			for (int i = (int)message.set_.offset; count-- > 0 &&
				// Make sure we still have space for the data.
				i < maxSoftkeySetDefinitions &&
				i < message.set_.total; ++i)
			{
				// If already defined, remove from hash table.
				lock (this_.device.SappStatusDataInfo.SoftkeySets.SyncRoot)
				{
					this_.device.SappStatusDataInfo.SoftkeySets.Remove(i);

					// Add to hash table.
					this_.device.SappStatusDataInfo.SoftkeySets.Add(i,
						message.set_.definitions[i]);

					// Maintain count as if softkeySets not a sparse array.
					if (i + 1 > this_.device.SappStatusDataInfo.SoftkeySetsCount)
					{
						this_.device.SappStatusDataInfo.SoftkeySetsCount = i + 1;
					}
				}
			}

			this_.LogVerbose("Reg: {0}: softkeySetCount: {1}",
				this_, this_.deviceData.SoftkeySetCount);

			// Request line enumeration only after we have all softkey sets.
			if (this_.deviceData.SoftkeySetCount == message.set_.total)
			{
				this_.ProcessButtons(ref followupEvents);
			}
		}

		/// <summary>
		/// Copies data from the LineStat message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for LineStat.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleLineStatResponse(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			LineStat message = event_.EventMessage as LineStat;

			// Make sure the line was defined in the template.
			if (message.line.number - 1 <= this_.deviceData.LineCount)
			{
				this_.LogVerbose(
					"Reg: {0}: line {1} defined, directoryNumber: {2}, displayName: {3}",
					this_, message.line.number, message.line.directoryNumber,
					message.line.fullyQualifiedDisplayName);

				lock (this_.device.SappStatusDataInfo.Lines.SyncRoot)
				{
					if (this_.device.SappStatusDataInfo.Lines.Count < maxLines)
					{
						this_.device.SappStatusDataInfo.Lines.Add(message.line);
					}
				}
			}
			else
			{
				this_.log.Write(TraceLevel.Warning,
					"Reg: {0}: line {1} not defined in template, directoryNumber: {2}, displayName: {3}; ignored",
					this_, message.line.number, message.line.directoryNumber,
					message.line.fullyQualifiedDisplayName);
			}

			this_.ProcessButtons(ref followupEvents);
		}

		/// <summary>
		/// Copies the data from the SpeeddialStat message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for SpeeddialStat.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleSpeeddialStatResponse(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			SpeeddialStat message = event_.EventMessage as SpeeddialStat;

			// Make sure the line was defined in the template.
			if (message.speeddial.number - 1 <= this_.deviceData.SpeeddialCount)
			{
				lock (this_.device.SappStatusDataInfo.Speeddials.SyncRoot)
				{
					if (this_.device.SappStatusDataInfo.Speeddials.Count <
						maxSpeeddials)
					{
						this_.device.SappStatusDataInfo.Speeddials.Add(
							message.speeddial);
					}
				}
			}

			this_.ProcessButtons(ref followupEvents);
		}

		/// <summary>
		/// Copies data from the FeatureStat message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for FeatureStat.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleFeatureStatResponse(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			FeatureStat message = event_.EventMessage as FeatureStat;

			// Make sure the line was defined in the template.
			if (message.feature.number - 1 <= this_.deviceData.FeatureCount)
			{
				lock (this_.device.SappStatusDataInfo.Features.SyncRoot)
				{
					if (this_.device.SappStatusDataInfo.Features.Count <
						maxFeatures)
					{
						this_.device.SappStatusDataInfo.Features.Add(
							message.feature);
					}
				}
			}

			this_.ProcessButtons(ref followupEvents);
		}

		/// <summary>
		/// Copies data from the ServiceUrlStat message.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for ServiceUrlStat.</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleServiceUrlStatResponse(
			StateMachine stateMachine, Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			ServiceUrlStat message = event_.EventMessage as ServiceUrlStat;

			// Make sure the line was defined in the template.
			if (message.serviceUrl.number - 1 <= this_.deviceData.ServiceUrlCount)
			{
				lock (this_.device.SappStatusDataInfo.ServiceUrls.SyncRoot)
				{
					if (this_.device.SappStatusDataInfo.ServiceUrls.Count <
						maxFeatures)
					{
						this_.device.SappStatusDataInfo.ServiceUrls.Add(
							message.serviceUrl);
					}
				}
			}

			this_.ProcessButtons(ref followupEvents);
		}

		/// <summary>
		/// Begins unregistration procedures.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">(Not used.)</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleUnregisterRequest(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;

			// The connection may have already been closed. If so, then just
			// pretend we received an UnregisterAck and cleanup.
			if (this_.Connected)
			{
				this_.Send(new Unregister());
			}
			else
			{
				// (Was using device.Discoverer.CallManager, but this method
				// seems to be safer, more direct. Don't really trust the old
				// method at all but decided to include it in case we can't
				// find CallManager the new way.)
				CallManager callManager;
				if (!this_.device.Discoverer.HaveCallManager(this_.RemoteEndPoint,
					out callManager))
				{
					callManager = this_.device.Discoverer.CallManager;
				}

				// (Under heavy load, was seeing callManager set to null
				// because list of CallManagers had been cleared, presumably
				// because Discovery.ResetDevice() had been called. This
				// workaround just assures that we don't try to use null for a
				// CallManager. I assume the corresponding CallManager object
				// has already been cleanly terminated, so no need to tear it
				// down.)
				if (callManager == null)
				{
					this_.log.Write(TraceLevel.Warning,
						"Reg: {0}: CallManager object mysteriously disappeared; UnregisterAck not sent to it", this_);
				}
				else
				{
					followupEvents.Enqueue(new Event((int)CallManager.EventType.UnregisterAck,
						callManager));
				}

				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
		}

		/// <summary>
		/// Handles CallManager acknowledging our unregister request.
		/// </summary>
		/// <param name="stateMachine">StateMachine to which to apply this event.</param>
		/// <param name="event_">Event for UnregisterAck</param>
		/// <param name="followupEvents">Followup events.</param>
		[Action()]
		private static void HandleUnregisterAck_(StateMachine stateMachine,
			Event event_, ref Queue followupEvents)
		{
			Registration this_ = stateMachine as Registration;
			UnregisterAck message = event_.EventMessage as UnregisterAck;

			// (Was using device.Discoverer.CallManager, but this method
			// seems to be safer, more direct. Don't really trust the old
			// method at all but decided to include it in case we can't
			// find CallManager the new way.)
			CallManager callManager;
			if (!this_.device.Discoverer.HaveCallManager(this_.RemoteEndPoint,
				out callManager))
			{
				callManager = this_.device.Discoverer.CallManager;
			}

			if (callManager == null)
			{
				followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
			}
			else
			{
				followupEvents.Enqueue(new Event((int)CallManager.EventType.UnregisterAck, callManager));

				if (message.status == UnregisterAck.Status.Ok)
				{
					followupEvents.Enqueue(new Event((int)EventType.Cleanup, this_));
				}
				else
				{
					// CallManager does not allow the unregistration, so go
					// back to the Registered state.
					followupEvents.Enqueue(new Event((int)EventType.Registered, this_));
				}
			}
		}

		#region Support methods to the HandleX methods
		/// <summary>
		/// Determines whether to request more button information or to finish
		/// registration.
		/// </summary>
		/// <param name="followupEvents">Followup events.</param>
		private void ProcessButtons(ref Queue followupEvents)
		{
			if (tempLineCount < deviceData.LineCount)
			{
				Send(new LineStatReq(++tempLineCount));
			}
			else if (tempSpeeddialCount < deviceData.SpeeddialCount)
			{
				Send(new SpeeddialStatReq(++tempSpeeddialCount));
			}
			else if (tempFeatureCount < deviceData.FeatureCount)
			{
				Send(new FeatureStatReq(++tempFeatureCount));
			}
			else if (tempServiceUrlCount < deviceData.ServiceUrlCount)
			{
				Send(new ServiceUrlStatReq(++tempServiceUrlCount));
			}
			else
			{
				Send(new RegisterAvailableLines(deviceData.LineCount));

				Send(new TimeDateReq());

				// (Was using device.Discoverer.CallManager, but this method
				// seems to be safer, more direct. Don't really trust the old
				// method at all but decided to include it in case we can't
				// find CallManager the new way.)
				CallManager callManager;
				if (!device.Discoverer.HaveCallManager(RemoteEndPoint,
					out callManager))
				{
					callManager = device.Discoverer.CallManager;
				}

				// (Just in case there is no longer a CallManager object.)
				if (callManager == null)
				{
					log.Write(TraceLevel.Warning,
						"Reg: {0}: CallManager object mysteriously disappeared; Registered not sent to it", this);
				}
				else
				{
					followupEvents.Enqueue(new Event((int)CallManager.EventType.Registered, callManager));
				}

				followupEvents.Enqueue(new Event((int)EventType.Registered, this));
			}
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
					"Reg: {0}: attempt to Send {1} on a null connection; ignored",
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
						"Reg: {0}: attempt to access LocalEndPoint on a null connection; using 0.0.0.0:0",
						this);

					// In case connection is null, return something.
					endPoint = new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0);
				}

				return endPoint;
			}
		}

		/// <summary>
		/// Returns IP address of remote end of connection to CallManager if
		/// there is still a connection to it.
		/// </summary>
		private IPEndPoint RemoteEndPoint
		{
			get
			{
				IPEndPoint endPoint;

				try
				{
					endPoint = connection.RemoteEndPoint;
				}
				catch (NullReferenceException)
				{
					log.Write(TraceLevel.Warning,
						"Reg: {0}: attempt to access RemoteEndPoint on a null connection; using 0.0.0.0:0",
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
					log.Write(TraceLevel.Warning,
						"Reg: {0}: attempt to determine whether Connected on a null connection; I guess not",
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
			return device == null ? "Registration" : device.ToString();
		}
	}
}
