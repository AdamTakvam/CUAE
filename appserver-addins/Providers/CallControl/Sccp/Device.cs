using System;
using System.Net;
using System.Diagnostics;
using System.Collections;

using Metreos.Core;
using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Utilities.Collections;
using Metreos.SccpStack;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.CallControl.Sccp;

namespace Metreos.CallControl.Sccp
{
	/// <summary>
	/// Represents an SCCP device, on which calls occur sequentially, never in
	/// parallel.
	/// </summary>
	/// <remarks>
	/// This class is concerned with device registration, constructing and
	/// destructing calls, and presenting a call facade to the Devices
	/// collection.
	/// </remarks>
	public class Device
	{
		/// <summary>
		/// Constructs an SCCP Device.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="device">Stack device associated with this
		/// Device.</param>
		/// <param name="configUtility">For accessing configuration
		/// data.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		/// <param name="devices">Encapsulation of collection that contains
		/// entries for all devices.</param>
		/// <param name="registeredDevices">Collection of Devices that are
		/// registered with a CallManager.</param>
		/// <param name="engagedDevices">Collection of Devices that are
		/// engaged in a Call.</param>
		/// <param name="recentlyEngagedDevices">Fixed-size collection of Device MAC
		/// addresses for the last N terminated calls.</param>
		/// <param name="registrationThrottle">Controls the sending of
		/// OpenDeviceRequest and CloseDeviceRequest messages to the stack so
		/// that the CallManager is never overwhelmed.</param>
		public Device(SccpProvider provider, string macAddress, SccpStack.Device device,
			IConfigUtility configUtility, LogWriter log, Devices devices,
			RegisteredDevices registeredDevices, EngagedDevices engagedDevices,
			BoundedCollection recentlyEngagedDevices,
			ProcessObject registrationThrottle)
		{
			Assertion.Check(device != null,
				"SccpStack: cannot create Device with null SccpStack.Device");

			this.provider = provider;
			this.log = log;
			this.macAddress = macAddress;
			this.device = device;
			this.configUtility = configUtility;
			this.devices = devices;
			this.registeredDevices = registeredDevices;
			this.engagedDevices = engagedDevices;
			this.recentlyEngagedDevices = recentlyEngagedDevices;
			this.registrationThrottle = registrationThrottle;

			// CallManager hasn't assigned a directory number to this device
			// yet.
			directoryNumber = null;

			// A Device begins life unregistered.
			registration = RegistrationState.Unregistered;

			// Subscribe to the two SCCP-stack events that affect registration.
			device.RegisteredEvent += new ClientMessageHandler(OnRegistered);
			device.UnregisteredEvent += new ClientMessageHandler(OnUnregistered);

			// We have not yet received a CallState(CallRemoteMultiline) message.
			lastCallRemoteMultilineCallId = 0;

			confirmed = true;

			activeCall = null;
		}

		/// <summary>
		/// Internal registration states for a Device.
		/// </summary>
		private enum RegistrationState
		{
			#region Main states

			/// <summary>
			/// Typically just the initial, idle state since we always try to
			/// stay registered.
			/// </summary>
			Unregistered,

			/// <summary>
			/// Asked stack to register; waiting for response.
			/// </summary>
			PendingRegistration,

			/// <summary>
			/// Normal, registered state.
			/// </summary>
			Registered,

			/// <summary>
			/// Asked stack to unregister; waiting for response.
			/// </summary>
			PendingUnregistration,

			#endregion

			#region Pre-empting states

			// The pre-empting states handle scenarios where requesting events
			// overlap each other (before resolution). All possible pre-empting
			// scenarios collapse down to the states listed here.

			/// <summary>
			/// Want to pre-empt pending registration with new, possibly
			/// different one.
			/// </summary>
			PendingRegistrationWhilePendingRegistration,

			/// <summary>
			/// Want to unregister after just asking to register.
			/// </summary>
			PendingUnregistrationWhilePendingRegistration,

			/// <summary>
			/// Want to register after just asking to unregister.
			/// </summary>
			PendingRegistrationWhilePendingUnregistration,

			// (There is no PendingUnregistrationWhilePendingUnregistration
			// state because one provider Unregister event is as good as
			// another--Unregister events are equivalent, unlike Register
			// events which may include different sets of CallManager
			// addresses.)

			#endregion
		}

		/// <summary>
		/// Current registration state for this Device.
		/// </summary>
		/// <remarks>
		/// There are only four events applied to this finite-state
		/// variable--Registered and Unregistered from the stack and
		/// Register and Unregister from the provider.
		/// </remarks>
		private RegistrationState registration;

		/// <summary>
		/// Property whose value is the current registration state for this
		/// Device.
		/// </summary>
		/// <remarks>
		/// Updates the Device's entry in the database when property is set.
		/// </remarks>
		private RegistrationState Registration
		{
			get { return registration; }
			set
			{
				switch (value)
				{
					case RegistrationState.Registered:
						configUtility.UpdateDeviceStatus(macAddress,
							IConfig.DeviceType.Sccp,
							IConfig.Status.Enabled_Running);
						break;

					case RegistrationState.Unregistered:
						configUtility.UpdateDeviceStatus(macAddress,
							IConfig.DeviceType.Sccp,
							IConfig.Status.Enabled_Stopped);
						break;

					default:
						// Do nothing.
						break;
				}
				
				// If we are transitioning out of the registered state, clear
				// the directory number in the database.
				if (registration == RegistrationState.Registered &&
					value != RegistrationState.Registered)
				{
					configUtility.SetDirectoryNumber(macAddress,
						IConfig.DeviceType.Sccp, SccpProvider.NotADirectoryNumber);
				}

				registration = value;
			}
		}

		/// <summary>
		/// Whether this device is registered with a CallManager.
		/// </summary>
		internal bool IsRegistered { get { return registration == RegistrationState.Registered; } }

		/// <summary>
		/// Stack device associated with this Device.
		/// </summary>
        private readonly SccpStack.Device device;

		/// <summary>
		/// MAC address.
		/// </summary>
		private string macAddress;

		/// <summary>
		/// Property whose value is this Device's MAC address.
		/// </summary>
        public string MacAddress { get { return macAddress; } }

		/// <summary>
		/// Directory number assigned by the CallManager and set upon
		/// registration.
		/// </summary>
		private string directoryNumber;

		/// <summary>
		/// Property whose value is the directory number from the CallManager.
		/// </summary>
		public string DirectoryNumber { get { return directoryNumber; } }

        /// <summary>
        /// Whether this Device survives the current refresh cycle.
        /// </summary>
        private bool confirmed;

		/// <summary>
		/// Property whose value is whether this Device survives the current
		/// refresh cycle.
		/// </summary>
        public bool Confirmed { get { return confirmed; } set { confirmed = value; } }

		/// <summary>
		/// Set to a Call when this Device has an active call; null,
		/// otherwise.
		/// </summary>
        private Call activeCall;

		/// <summary>
		/// Property whose value is a Call when this Device has an active
		/// call; null, otherwise.
		/// </summary>
        public Call Call { get { return activeCall; } }

		/// <summary>
		/// Property whose value is whether this Device is in use--has an
		/// active call.
		/// </summary>
        public bool InUse { get { return activeCall != null; } }

		/// <summary>
		/// For accessing configuration data.
		/// </summary>
		private readonly IConfigUtility configUtility;

		/// <summary>
		/// SccpProvider.
		/// </summary>
		protected readonly SccpProvider provider;

		/// <summary>
		/// Where diagnostics are written to.
		/// </summary>
		private readonly LogWriter log;

		/// <summary>
		/// Encapsulation of collections that contain entries for all Devices.
		/// </summary>
		private readonly Devices devices;

		/// <summary>
		/// The collection that contains entries for all registered Devices,
		/// indexed by directory number.
		/// </summary>
		private readonly RegisteredDevices registeredDevices;

		/// <summary>
		/// The collection that contains entries for all Devices engaged in a
		/// call, indexed by call id.
		/// </summary>
		private readonly EngagedDevices engagedDevices;

		/// <summary>
		/// Fixed-size collection of Device MAC addresses for the last N
		/// terminated calls.
		/// </summary>
		private readonly BoundedCollection recentlyEngagedDevices;

		/// <summary>
		/// Controls the sending of OpenDeviceRequest and CloseDeviceRequest
		/// messages to the stack so that the CallManager is never overwhelmed.
		/// </summary>
		private readonly ProcessObject registrationThrottle;

		/// <summary>
		/// CallManager addresses (primary, secondary, etc.).
		/// </summary>
		private ArrayList callManagerAddresses;

		/// <summary>
		/// List of SRST IPEndPoints which this device may use for recovery.
		/// </summary>
		/// <remarks>(SRST stands for Survivable Remote Site Telephony.)</remarks>
		private ArrayList srstAddresses;

		/// <summary>
		/// Call id (different from the TM callId) from the most recent
		/// CallState(CallRemoteMultiline). Barge needs it.
		/// </summary>
		private uint lastCallRemoteMultilineCallId;

		/// <summary>
		/// Property whose value is the call id from the most recent
		/// CallState(CallRemoteMultiline).
		/// </summary>
		public uint LastCallRemoteMultilineCallId
		{ set { lastCallRemoteMultilineCallId = value; } }

		/// <summary>
		/// Dummy object just to do locks on so that we don't collide when
		/// changing the association between this Device and a Call.
		/// </summary>
		/// <remarks>
		/// Adam heard that from Microsoft, I believe, that it is faster to
		/// lock on something at the same level, e.g., our object here, rather
		/// than "this," itself.
		/// </remarks>
		private readonly object associationLock = new object();

		/// <summary>
		/// Disassociates the active call from this Device.
		/// </summary>
		public void RemoveCall()
		{
			// Add this Device to recentlyEngagedDevices collection before
			// Disassociating with Call so that there will be some overlap.
			recentlyEngagedDevices.Add(macAddress);

			Call call = Disassociate();
			if (call != null)
			{
				if (provider.IsLogCallVerbose)
				{
					log.Write(TraceLevel.Verbose,
						"Prv: {0}: cleared call {1}", this, call.CallId);
				}
			}
		}

		/// <summary>
		/// Returns call id of this Device if it has an active call;
		/// 0 otherwise.
		/// </summary>
		/// <returns>Unique identifier of this call for use between the
		/// Telephony Manager and the provider; 0 if no active call.</returns>
		public long GetCallId()
		{
			Call call = Call;
			return call != null ? call.CallId : 0;
		}

		/// <summary>
		/// Triggers an event on the Device's Call state-machine,
		/// if present.
		/// </summary>
		/// <param name="id">Event identifier.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool CallTrigger(int id)
		{
			bool triggered = false;

			Call call = Call;
			if (call != null)
			{
				StateMachine.ProcessEvent(new Event(id, call));
				triggered = true;
			}

			return triggered;
		}

		/// <summary>
		/// Triggers an event on the Device's Call state-machine,
		/// if present, and passes along the message from the SCCP stack that
		/// caused this event.
		/// </summary>
		/// <param name="id">Event identifier.</param>
		/// <param name="msg">Message from the SCCP stack.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool CallTrigger(int id, Metreos.SccpStack.Message msg)
		{
			bool triggered = false;

			Call call = Call;
			if (call != null)
			{
				StateMachine.ProcessEvent(new Event(id, msg, call));
				triggered = true;
			}

			return triggered;
		}

		/// <summary>
		/// Triggers a Redirect event on the attached Call.
		/// </summary>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool RedirectCall()
		{
			return CallTrigger((int)Call.EventType.Redirect);
		}

		/// <summary>
		/// Triggers a Reject event on the attached Call.
		/// </summary>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool RejectCall()
		{
			return CallTrigger((int)Call.EventType.Reject);
		}

		/// <summary>
		/// Triggers a Hangup event on the attached Call.
		/// </summary>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool HangupCall()
		{
			return CallTrigger((int)Call.EventType.Hangup);
		}

		/// <summary>
		/// Triggers a ReceivedRelease event on the attached Call.
		/// </summary>
		/// <param name="releaseMessage">Release message.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ReceivedRelease(Release releaseMessage)
		{
			return CallTrigger((int)Call.EventType.ReceivedRelease, releaseMessage);
		}

		/// <summary>
		/// Triggers a ReceivedReleaseComplete event on the attached Call.
		/// </summary>
		/// <param name="releaseCompleteMessage">ReleaseComplete message.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ReceivedReleaseComplete(ReleaseComplete releaseCompleteMessage)
		{
			return CallTrigger((int)Call.EventType.ReceivedReleaseComplete, releaseCompleteMessage);
		}

        #region Registration/Unregistration

		/// <summary>
		/// Initiates the registration process for this Device according to the
		/// specified set of CallManager addresses.
		/// </summary>
		/// <param name="callManagerIPAddresses">CallManager addresses
		/// (primary, secondary, etc.).</param>
		/// <param name="srstAddresses">List of SRST addresses.</param>
		/// <param name="callManagerPort">Port on which CallManagers are
		/// listening for registrations.</param>
		/// <returns>Whether the CallManager addresses are usable for
		/// registration.</returns>
        public bool Register(IPAddress[] callManagerIPAddresses,
			IPAddress[] srstIPAddresses, int callManagerPort)
        {
			bool registerRequested = false;

			if (callManagerIPAddresses != null && callManagerIPAddresses.Length > 0)
			{
				// Create list of CallManager addresses for this Device for this
				// registration and possible subsequent re-registrations.
				callManagerAddresses = new ArrayList();
				foreach (IPAddress callManagerIPAddress in callManagerIPAddresses)
				{
					callManagerAddresses.Add(new IPEndPoint(callManagerIPAddress,
						callManagerPort));
				}

				// Create list of SRST addresses for this Device for this
				// registration and possible subsequent re-registrations.
				srstAddresses = new ArrayList();
				foreach (IPAddress srstIPAddress in srstIPAddresses)
				{
					srstAddresses.Add(new IPEndPoint(srstIPAddress,
						callManagerPort));
				}

				// Based on the current registration state, decide what to do
				// next to ultimately register this Device.
				switch (Registration)
				{
					case RegistrationState.Unregistered:
						// Ask the stack to register this Device. This is the
						// nominal transition for this event.
						Registration = RegistrationState.PendingRegistration;
						SendOpenDeviceRequest();
						break;

					case RegistrationState.PendingRegistration:
						// We are pre-empting a previous registration request
						// before the stack has responded. Just change state so
						// that we will know how to act once we get a response
						// from the stack.
						Registration = RegistrationState.PendingRegistrationWhilePendingRegistration;
						break;

					case RegistrationState.Registered:
						// We were registered but now we want to re-register
						// with a (presuambly) new list of CallManagers. Start
						// the process off by asking the stack to unregister
						// us.
						Registration = RegistrationState.PendingRegistrationWhilePendingUnregistration;
						SendCloseDeviceRequest();
						break;

					case RegistrationState.PendingUnregistration:
						// We had asked the stack to unregister us, but we
						// changed our minds. Now we want to register (with a
						// possibly new list of CallManagers) once the stack
						// responds to our outstanding unregistration request.
						Registration = RegistrationState.PendingRegistrationWhilePendingUnregistration;
						break;

					case RegistrationState.PendingRegistrationWhilePendingRegistration:
						// No state transition.

						// Geez, we can't make up our minds. We had already
						// pre-empted a registration with another (stack had
						// not responded to the request yet). Now we have yet
						// another registration (with a presumably new list of
						// Callmanagers). The list of CallManagers has been
						// updated and will used once we are able to request a
						// new registration.
						break;

					case RegistrationState.PendingUnregistrationWhilePendingRegistration:
						// We had asked the stack to register this Device but
						// it had not yet responded, then we decided that we
						// didn't want to be registered after all. Now we have
						// changed our minds again and want to be registered
						// with the possibly new list of CallManagers that we
						// just saved, above.
						Registration = RegistrationState.PendingRegistration;
						break;

					case RegistrationState.PendingRegistrationWhilePendingUnregistration:
						// No state transition.

						// We had requested unregistration but wanted to
						// register with a (presumably) new list of
						// CallManagers. While we are still waiting for the
						// stack to respond to the original unregistration
						// request, we have a (presumably) new list of
						// CallManagers with which we want to register.
						break;

					default:
						Debug.Fail("SccpStack: register attempt in unexpected registration state: " +
							Registration.ToString());
						break;
				}

				registerRequested = true;
			}
			else
			{
				log.Write(TraceLevel.Warning,
					"Prv: {0}: no CallManagers with which to register; ignored",
					this);
			}
			
			return registerRequested;
        }

		/// <summary>
		/// Initiates the unregistration process for this Device.
		/// </summary>
		/// <param name="expedite">Whether to expedite unregistering this
		/// device, i.e., not waiting for a response from the CM.</param>
		public void Unregister(bool expedite)
        {
			// (We might do something else in the future, but for now let's
			// just don't do anything here for expedited unregistration.)
			if (!expedite)
			{
				// If we still have an active call on this Device, hang up and nuke
				// the call.
				Call call = activeCall;
				if (call != null)
				{
					StateMachine.ProcessEvent(new Event((int)Call.EventType.Hangup, call));
				}

				// Based on the current registration state, decide what to do
				// next to ultimately unregister this Device.
				switch (Registration)
				{
					case RegistrationState.Unregistered:
						// No state transition.

						// Shouldn't happen but doesn't hurt--we are already
						// unregistered.
						break;

					case RegistrationState.PendingRegistration:
						// We were pending registration but now decided that we
						// didn't want to be registered after all. Once we get a
						// response from the registration request, make sure we are
						// unregistered.
						Registration = RegistrationState.PendingUnregistrationWhilePendingRegistration;
						break;

					case RegistrationState.Registered:
						// Ask the stack to unregister this Device. This is the
						// nominal transition for this event.
						Registration = RegistrationState.PendingUnregistration;
						SendCloseDeviceRequest();
						break;

					case RegistrationState.PendingUnregistration:
						// No state transition.

						// Shouldn't happen but doesn't hurt--we are already in the
						// process of unregistering when asked to unregister again.
						break;

					case RegistrationState.PendingRegistrationWhilePendingRegistration:
						// We had pre-empted a pending registration with another
						// but now decided that we didn't want to be registered
						// after all. Once we get a response from the registration
						// request, make sure we are unregistered.
						Registration = RegistrationState.PendingUnregistrationWhilePendingRegistration;
						break;

					case RegistrationState.PendingUnregistrationWhilePendingRegistration:
						// No state transition.

						// Shouldn't happen but doesn't hurt--we are already in the
						// process of unregistering (after pre-empting a
						// registration request) when asked to unregister again.
						break;

					case RegistrationState.PendingRegistrationWhilePendingUnregistration:
						// We had pre-empted an unregistration request with the
						// desire to become registered again, but now we want to be
						// unregistered. Simply go back to waiting for the stack to
						// respond to the original unregistration request.
						Registration = RegistrationState.PendingUnregistration;
						break;

					default:
						Debug.Fail("SccpStack: unregister attempt in unexpected registration state: " +
							Registration.ToString());
						break;
				}
			}
        }

        #endregion

        #region CallControl methods

		/// <summary>
		/// Creates an IncomingCall as long as this Device does not already
		/// have an active call.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		/// <param name="displayName">Display name of caller.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		public void CreateIncomingCall(SccpProvider provider, long callId,
			string to, string from, string originalTo, string displayName, LogWriter log)
		{
			Assertion.Check(!InUse,
				"SccpStack: cannot create call that is already in use");

			Associate(new IncomingCall(provider, log, callId, to, from,
				originalTo, displayName));

			// Remove this Device from recentlyEngagedDevices collection after
			// Associating with Call so that there will be some overlap.
			recentlyEngagedDevices.Remove(macAddress);
		}

		/// <summary>
		/// Creates an OutgoingCall as long as this Device does not already
		/// have an active call.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="outCallInfo">Call metadata from app/TM action.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		public void CreateOutgoingCall(SccpProvider provider,
			OutboundCallInfo outCallInfo, LogWriter log)
		{
			Assertion.Check(!InUse,
				"SccpStack: cannot create call that is already in use");

			Associate(new OutgoingCall(provider, log, outCallInfo));

			// Remove this Device from recentlyEngagedDevices collection after
			// Associating with Call so that there will be some overlap.
			recentlyEngagedDevices.Remove(macAddress);
		}

		/// <summary>
		/// Creates a BargedCall as long as this Device does not already have
		/// an active call.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		public void CreateBargedCall(SccpProvider provider, long callId,
			string to, string from, string originalTo, LogWriter log)
		{
			Associate(new BargedCall(provider, log, callId, to, from,
				originalTo, lastCallRemoteMultilineCallId));

			// Remove this Device from recentlyEngagedDevices collection after
			// Associating with Call so that there will be some overlap.
			recentlyEngagedDevices.Remove(macAddress);
		}

        #endregion

		/// <summary>
		/// Associates a Call with this Device (they refer to each other).
		/// </summary>
		/// <param name="call">Call to associate with this Device.</param>
		/// <returns>Whether a Call (not null) was associated with this Device.</returns>
		private bool Associate(Call call)
		{
			bool associated = false;

			if (call != null)
			{
				lock (engagedDevices.SyncRoot)
				{
					lock (associationLock)
					{
						Assertion.Check(activeCall == null, "SccpStack: device already associated with call");
						Assertion.Check(call.Device == null, "SccpStack: call already associated with device");

						activeCall = call;
						call.Device = this;

						engagedDevices.Add(call.CallId, this);

						associated = true;
					}
				}
			}

			return associated;
		}

		/// <summary>
		/// Breaks the association between this Device and its Call (removes
		/// the references to each other) and removes the call from the
		/// engagedDevices collection.
		/// </summary>
		/// <returns>The Call with which this Device was associated; null
		/// otherwise.</returns>
		private Call Disassociate()
		{
			Call disassociatedCall = activeCall;

			if (activeCall != null)
			{
				lock (engagedDevices.SyncRoot)
				{
					lock (associationLock)
					{
						Assertion.Check(activeCall.Device != null, "SccpStack: missing device");
						Assertion.Check(activeCall.Device == this, "SccpStack: call not associated with device");

						activeCall.Device = null;
						activeCall = null;

						engagedDevices.Remove(disassociatedCall.CallId);
					}
				}
			}

			return disassociatedCall;
		}

		/// <summary>
		/// Moves a Call from one Device to another.
		/// </summary>
		/// <param name="fromDevice">Device whose Call is being moved.</param>
		/// <param name="toDevice">Device that will receive the Call being moved.</param>
		/// <returns>Whether the Call was moved.</returns>
		public static bool MoveCall(Device fromDevice, Device toDevice)
		{
			Assertion.Check(fromDevice != null, "SccpStack: missing from device");
			Assertion.Check(toDevice != null, "SccpStack: missing to device");
			Assertion.Check(fromDevice.activeCall != null, "SccpStack: missing Call in from Device");
			Assertion.Check(toDevice.activeCall == null, "SccpStack: to Device already has Call");

			return toDevice.Associate(fromDevice.Disassociate());
		}

        #region Callbacks from SCCP stack device

		/// <summary>
		/// Handles the Registered event from the SCCP stack--the CallManager
		/// has accepted a prior registration request from this Device.
		/// </summary>
		/// <param name="device">Device from which this message was sent
		/// from the stack.</param>
		/// <param name="message">The Registered message from SCCP stack.</param>
        private void OnRegistered(SccpStack.Device device, ClientMessage message)
        {
            Registered regMessage = message as Registered;
            Assertion.Check(regMessage != null,
				"SccpStack: invalid message type in Registered event");

			// Store directory number, assigned by the CallManager, in database.
			ArrayList lines = regMessage.lines;
			if (lines != null && lines.Count > 0)
			{
				directoryNumber = ((Line)lines[0]).directoryNumber;

				log.Write(TraceLevel.Info,
					"Prv: {0}: SCCP device {1} registered (while {2})",
					this, directoryNumber, Registration);

				registeredDevices[directoryNumber] = this;

				// Based on the current registration state, decide what to do
				// next now that we are registered.
				switch (Registration)
				{
					case RegistrationState.Unregistered:
						// Don't know how we could have gone directly from
						// unregistered to registered state, but, hey, let's go
						// with the flow.
						Registration = RegistrationState.Registered;
						break;

					case RegistrationState.PendingRegistration:
						// Yay, we got a positive response for our registration
						// request! This is the nominal transition for this event.
						Registration = RegistrationState.Registered;
						break;

					case RegistrationState.Registered:
						// No state transition.

						// Shouldn't happen--we're already registered--but
						// shouldn't hurt either.
						break;

					case RegistrationState.PendingUnregistration:
						// No state transition.

						// This is unusual--we're waiting to unregister when the
						// stack tells us that we're registered. Let's request
						// unregistration again. NOTE: There is a risk here of
						// getting into a please-unregister-hey-you're-registered
						// loop.
						SendCloseDeviceRequest();
						break;

					case RegistrationState.PendingRegistrationWhilePendingRegistration:
						Registration = RegistrationState.PendingRegistrationWhilePendingUnregistration;

						// We wanted to pre-empt a pending registration with
						// (presumably) a new list of CallManagers. Now that we
						// have received notice from the stack that the pending
						// registration has succeeded, let's unregister so that we
						// can register with the new set of CallManagers.
						SendCloseDeviceRequest();
						break;

					case RegistrationState.PendingUnregistrationWhilePendingRegistration:
						// We had been waiting for a response from the stack to our
						// previous registration request when we changed our minds
						// and decided that we didn't want to be registered after
						// all. Now that the stack has told us that our previous
						// registration request has succeeded, let's ask it to
						// unregister.
						Registration = RegistrationState.PendingUnregistration;
						SendCloseDeviceRequest();
						break;

					case RegistrationState.PendingRegistrationWhilePendingUnregistration:
						// No state transition.

						// This is unusual--we're waiting to unregister when the
						// stack tells us that we're registered. Let's request
						// unregistration again. NOTE: There is a risk here of
						// getting into a please-unregister-hey-you're-registered
						// loop.
						SendCloseDeviceRequest();
						break;

					default:
						Debug.Fail("SccpStack: became registered in unexpected registration state: " +
							Registration.ToString());
						break;
				}
			}
			else
			{
				directoryNumber = SccpProvider.NotADirectoryNumber;
			}
			configUtility.SetDirectoryNumber(macAddress,
				IConfig.DeviceType.Sccp, directoryNumber);
		}

		/// <summary>
		/// Handles the Registered event from the SCCP stack--the CallManager
		/// has accepted a prior registration request from this Device.
		/// </summary>
		/// <remarks>
		/// Stack calls this function when Device becomes unregistered. We
		/// immediately request that it re-register.
		/// </remarks>
		/// <param name="device">Device from which this message was sent
		/// from the stack.</param>
		/// <param name="message">The Registered message from SCCP stack.
		/// Ignored.</param>
		private void OnUnregistered(SccpStack.Device device, ClientMessage message)
		{
			log.Write(TraceLevel.Info,
				"Prv: {0}: SCCP device {1}unregistered (while {2})",
				this, directoryNumber == null ? "" : (directoryNumber.ToString() + " "), Registration);

			// If inexplicably still set to null, don't try to remove it from
			// DB because would cause exception.
			if (directoryNumber != null)
			{
				registeredDevices.Remove(directoryNumber);
				directoryNumber = null;
			}

			// Based on the current registration state, decide what to do
			// next now that we are unregistered.
			switch (Registration)
			{
				case RegistrationState.Unregistered:
					// No state transition.

					// the stack is telling us something we already know.
					// Shouldn't ever happen but doesn't hurt if it does.
					break;

				case RegistrationState.PendingRegistration:
					// No state transition.

					// Our registration request has failed. We probably lost
					// our connection to the CallManager, so let's try
					// registering again. This could go on indefinitely, but we
					// don't have anything better to do, so just keep trying.
					SendOpenDeviceRequest();
					break;

				case RegistrationState.Registered:
					// We have become unregistered even though we didn't ask to
					// be. We probably lost our connection to the CallManager,
					// so let's try registering again. This could go on
					// indefinitely, but we don't have anything better to do,
					// so just keep trying.
					Registration = RegistrationState.PendingRegistration;
					SendOpenDeviceRequest();
					break;

				case RegistrationState.PendingUnregistration:
					// Yay, we got a positive response for our unregistration
					// request! This is the nominal transition for this event.
					Registration = RegistrationState.Unregistered;
					break;

				case RegistrationState.PendingRegistrationWhilePendingRegistration:
					// A registration request has failed that we were wanting
					// to pre-empt with a new one anyway. We probably lost our
					// connection to the CallManager. Let's try registering
					// again but now with the new list of CallManagers.
					Registration = RegistrationState.PendingRegistration;
					SendOpenDeviceRequest();
					break;

				case RegistrationState.PendingUnregistrationWhilePendingRegistration:
					// We had pre-empted a registration request with a desire
					// to not be registered after all. That previous
					// registration request has now failed, so there is now no
					// need to explicitly unregister. (Yay!) We'll just quietly
					// go back to the unregistered state and wait for further
					// instructions.
					Registration = RegistrationState.Unregistered;
					break;

				case RegistrationState.PendingRegistrationWhilePendingUnregistration:
					// We had requested unregistration but now we want to
					// register either because we changed our minds or we want
					// to register with a (presumably) new list of
					// CallManagers. Now that the unregistration has succeeded,
					// let's register with the new list of CallManagers.
					Registration = RegistrationState.PendingRegistration;
					SendOpenDeviceRequest();
					break;

				default:
					Debug.Fail("SccpStack: became unregistered in unexpected registration state: " +
						Registration.ToString());
					break;
			}
		}

		/// <summary>
		/// Sends OpenDeviceRequest message to SCCP stack to request
		/// registration.
		/// </summary>
		private void SendOpenDeviceRequest()
		{
			if (callManagerAddresses == null || srstAddresses == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: no CallManager or SRST addresses; cannot re-register",
					this);
			}
			else
			{
				registrationThrottle.Submit(new OpenDeviceRequest(this,
					callManagerAddresses, srstAddresses));
			}
		}

		/// <summary>
		/// Send CloseDeviceRequest message to SCCP stack to request
		/// unregistration.
		/// </summary>
		private void SendCloseDeviceRequest()
		{
			registrationThrottle.Submit(new CloseDeviceRequest(this));
		}

		/// <summary>
		/// Sends SCCP message to CallManager if relationship still exists (it should).
		/// </summary>
		/// <param name="message">SCCP message.</param>
		internal void Send(SccpMessage message)
		{
			if (device != null)
			{
				device.Send(message);
			}
		}

		/// <summary>
		/// Sends internal message to SCCP stack if relationship still exists (it should).
		/// </summary>
		/// <param name="message">Internal message.</param>
		internal void Send(ClientMessage message)
		{
			if (device != null)
			{
				device.Send(message);
			}
		}

		internal bool HavePrimaryConnection(out SccpConnection primaryCallManager)
		{
            return device.HavePrimaryConnection(out primaryCallManager);
		}

        #endregion

		/// <summary>
		/// Returns a string that represents this object.
		/// </summary>
		/// <returns>String that represents this object.</returns>
		public override string ToString()
		{
			return macAddress;
		}
    }
}
