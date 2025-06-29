using System;
using System.Net;
using System.Collections;

using Metreos.Utilities;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents an internal SCCP call-control message abstraction.
	/// </summary>
	/// <remarks>
	/// These messages are never sent on the wire. Besides representing roughly
	/// equivalent Q.931 call-control messages, they also encapsulate the set
	/// of parameters for each action/event regardless of direction, i.e.,
	/// incoming or outgoing.
	/// </remarks>
	public abstract class ClientMessage : Message
	{
		/// <summary>
		/// Property whose value is the line number that this message
		/// references or 0 if not applicable.
		/// </summary>
		public virtual uint Line { get { return 0; } }

		/// <summary>
		/// Property whose value is the call id, or call reference, that this
		/// message references or 0 if not applicable.
		/// </summary>
		public virtual uint CallId { get { return 0; } }
	}

	/// <summary>
	/// Call has been accepted and is ringing (alerting).
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class Alerting : ClientMessage
	{
		/// <summary>
		/// Alerting for line 1.
		/// </summary>
		public Alerting() : this(1) { }

		/// <summary>
		/// Alerting for specified line number.
		/// </summary>
		/// <param name="lineNumber">Number of line alerting.</param>
		public Alerting(uint lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Client will no longer be sent media.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class CloseReceive : ClientMessage
	{
		/// <summary>
		/// Stop receiving media, using all defaults.
		/// </summary>
		public CloseReceive() : this(0, 0) { }

		/// <summary>
		/// Stop receiving media, using default line number of 1.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public CloseReceive(uint conferenceId,
			uint passthruPartyId) : this(1, conferenceId, passthruPartyId) { }

		/// <summary>
		/// Stop receiving media.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public CloseReceive(uint lineNumber, uint conferenceId,
			uint passthruPartyId)
		{
			this.lineNumber = lineNumber;
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Conference identifier. Identifies messages belonging to a
		/// particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier. Typically ties a response to a request so
		/// that the receiver of a response knows to which request a message is
		/// in response.
		/// </summary>
		public uint passthruPartyId;
	}

	/// <summary>
	/// Unregister with CallManager.
	/// </summary>
	/// <remarks>Send only.</remarks>
	public class CloseDeviceRequest : ClientMessage
	{
		/// <summary>
		/// Unregister with CallManager with "okay" cause.
		/// </summary>
		public CloseDeviceRequest() : this (Device.Cause.Ok) { }

		/// <summary>
		/// Unregister with CallManager.
		/// </summary>
		public CloseDeviceRequest(Device.Cause cause)
		{
			this.cause = cause;
		}

		/// <summary>
		/// Cause of device-close request.
		/// </summary>
		public Device.Cause cause;
	}

	/// <summary>
	/// Call has been answered.
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class Connect : ClientMessage
	{
		/// <summary>
		/// Connect on line 1.
		/// </summary>
		public Connect() : this(1) { }

		/// <summary>
		/// Connect for specified line number.
		/// </summary>
		/// <param name="lineNumber">Number of line connected.</param>
		public Connect(uint lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Acknowledgement to answerer that caller knows call has been answered.
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class ConnectAck : ClientMessage
	{
		/// <summary>
		/// Acknowledge call established on line 1.
		/// </summary>
		public ConnectAck() : this(1) { }

		/// <summary>
		/// Acknowledge call established.
		/// </summary>
		/// <param name="lineNumber">Number of line connected.</param>
		public ConnectAck(uint lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Sends user-related data, e.g., XML, to the client.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class DeviceToUserDataRequest : ClientMessage
	{
		/// <summary>
		/// Send user-related data from CallManager to client for line 1. The
		/// consumer must subsequently provide relevant information.
		/// </summary>
		public DeviceToUserDataRequest() : this(0, 1, null) { }

		/// <summary>
		/// Send user-related data from CallManager to client.
		/// </summary>
		/// <param name="callId">Uniquely identifies calls on the same device.</param>
		/// <param name="line">Line number.</param>
		/// <param name="data">Data sent by CallManager to client.</param>
		public DeviceToUserDataRequest(uint callId, uint line,
			UserAndDeviceData data)
		{
			this.callId = callId;
			this.line = line;
			this.data = data;
		}

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callId;

		/// <summary>
		/// Property whose value uniquely identifies calls on the same device.
		/// </summary>
		public override uint CallId { get { return callId; } }

		/// <summary>
		/// Line number.
		/// </summary>
		public uint line;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return line; } }

		/// <summary>
		/// User-related data.
		/// </summary>
		public UserAndDeviceData data;
	}

	/// <summary>
	/// Sends user-related data, e.g., XML, to the CallManager.
	/// </summary>
	/// <remarks>Send only.</remarks>
	public class DeviceToUserDataResponse : ClientMessage
	{
		/// <summary>
		/// Send user-related data from client to the CallManager for line 1.
		/// The consumer must subsequently provide relevant information.
		/// </summary>
		public DeviceToUserDataResponse() : this(0, 1, null) { }

		/// <summary>
		/// Send user-related data from client to the CallManager.
		/// </summary>
		/// <param name="callId">Uniquely identifies calls on the same device.</param>
		/// <param name="line">Line number.</param>
		/// <param name="data">Data sent by client to CallManager.</param>
		public DeviceToUserDataResponse(uint callId, uint line,
			UserAndDeviceData data)
		{
			this.callId = callId;
			this.line = line;
			this.data = data;
		}

		/// <summary>
		/// Uniquely identifies calls on the same device.
		/// </summary>
		public uint callId;

		/// <summary>
		/// Property whose value uniquely identifies calls on the same device.
		/// </summary>
		public override uint CallId { get { return callId; } }

		/// <summary>
		/// Line number.
		/// </summary>
		public uint line;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return line; } }

		/// <summary>
		/// User-related data.
		/// </summary>
		public UserAndDeviceData data;
	}

	/// <summary>
	/// Send "DTMF" digits.
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class Digits : ClientMessage
	{
		/// <summary>
		/// Convey digits on line 1.
		/// </summary>
		public Digits(string digits) : this(1, digits) { }

		/// <summary>
		/// Convey digits.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="digits">Digits to convey.</param>
		public Digits(uint lineNumber, string digits)
		{
			this.lineNumber = lineNumber;
			this.digits = digits;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		public string digits;
	}

	/// <summary>
	/// Feature request.
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class FeatureRequest : ClientMessage
	{
		/// <summary>
		/// Feature request on line 1.
		/// </summary>
		/// <param name="feature">Feature code.</param>
		public FeatureRequest(Feature feature) : this(1, feature) { }

		/// <summary>
		/// Feature request.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="feature">Feature code.</param>
		public FeatureRequest(uint lineNumber, Feature feature)
		{
			this.lineNumber = lineNumber;
			this.feature = feature;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Requested-feature code.
		/// </summary>
		public Feature feature;

		/// <summary>
		/// Codes for various types of features that a client may request.
		/// </summary>
		public enum Feature
		{
			None,
			Redial,
			NewCall,
			Hold,
			Transfer,
			CallForwardAll,
			CallForwardBusy,
			CallForwardNoAnswer,
			Backspace,
			EndCall,
			Resume,
			Answer,
			Info,
			Conference,
			Park,
			Join,
			MeetMeConference,
			CallPickup,
			GroupCallPickup,
			Drop,
			CallBack,
			Barge,
			Speeddial,
		}
	}

	/// <summary>
	/// Offhook. Called OffhookClient to distinguish from OffhookSccp.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class OffhookClient : ClientMessage
	{
		/// <summary>
		/// Offhook on line 1.
		/// </summary>
		public OffhookClient() : this(1) { }

		/// <summary>
		/// Offhook.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public OffhookClient(uint lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Other client ready to send media.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class OpenReceiveRequest : ClientMessage
	{
		/// <summary>
		/// Request to receive media (other side is ready to send media) on line 1.
		/// </summary>
		/// <param name="media">Media information.</param>
		public OpenReceiveRequest(MediaInfo media) : this(1, media) { }

		/// <summary>
		/// Request to receive media (other side is ready to send media).
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="media">Media information.</param>
		public OpenReceiveRequest(uint lineNumber, MediaInfo media)
		{
			this.lineNumber = lineNumber;
			this.media = media;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Describes media that this message is requesting to be received.
		/// </summary>
		public MediaInfo media;
	}

	/// <summary>
	/// Client is ready to receive media.
	/// </summary>
	/// <remarks>Send only.</remarks>
	public class OpenReceiveResponse : ClientMessage
	{
		/// <summary>
		/// Client is ready to receive media on line 1.
		/// </summary>
		/// <param name="media">Media information.</param>
		public OpenReceiveResponse(MediaInfo media) :
			this(1, media, Device.Cause.Ok) { }

		/// <summary>
		/// Client is ready to receive media.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="media">Media information.</param>
		/// <param name="cause">Cause.</param>
		public OpenReceiveResponse(uint lineNumber, MediaInfo media,
			Device.Cause cause)
		{
			this.lineNumber = lineNumber;
			this.media = media;
			this.cause = cause;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Describes media that this message can receive.
		/// </summary>
		public MediaInfo media;

		/// <summary>
		/// Cause of open-receive response.
		/// </summary>
		public Device.Cause cause;
	}

	/// <summary>
	/// Register with CallManager.
	/// </summary>
	/// <remarks>Send only.</remarks>
	public class OpenDeviceRequest : ClientMessage
	{
		/// <summary>
		/// Register with CallManager with just the MAC address and CallManager
		/// addresses.
		/// </summary>
		/// <param name="macAddress">Client's MAC address.</param>
		/// <param name="callManagerAddresses">IPEndPoint addresses of
		/// CallManagers (primary, secondary, etc.).</param>
		public OpenDeviceRequest(string macAddress,
			ArrayList callManagerAddresses) :
			this(macAddress,
			IpUtility.ResolveHostname(Dns.GetHostName()),
			callManagerAddresses, new ArrayList(),
			new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0),
			Discovery.DeviceType_, Discovery.Version) { }

		/// <summary>
		/// Register with CallManager with the MAC address, CallManager
		/// addresses, and SRST addresses.
		/// </summary>
		/// <param name="macAddress">Client's MAC address.</param>
		/// <param name="callManagerAddresses">IPEndPoint addresses of
		/// CallManagers (primary, secondary, etc.).</param>
		/// <param name="srstAddresses">List of SRST addresses.</param>
		public OpenDeviceRequest(string macAddress,
			ArrayList callManagerAddresses, ArrayList srstAddresses) :
			this(macAddress,
			IpUtility.ResolveHostname(Dns.GetHostName()),
			callManagerAddresses, srstAddresses,
			new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0),
			Discovery.DeviceType_, Discovery.Version) { }

		/// <summary>
		/// Register with CallManager without providing the less common
		/// parameters.
		/// </summary>
		/// <param name="macAddress">Client's MAC address.</param>
		/// <param name="clientAddress">Client's address.</param>
		/// <param name="callManagerAddresses">IPEndPoint addresses of
		/// CallManagers (primary, secondary, etc.).</param>
		/// <param name="deviceType">Device type, e.g., Station Telecaster
		/// Manager.</param>
		/// <param name="version">Version of client to report to
		/// CallManager.</param>
		public OpenDeviceRequest(string macAddress, IPAddress clientAddress,
			ArrayList callManagerAddresses,
			DeviceType deviceType, ProtocolVersion version) :
			this(macAddress, clientAddress, callManagerAddresses,
			new ArrayList(), new IPEndPoint(IPAddress.Parse("0.0.0.0"), 0),
			deviceType, version) { }

		/// <summary>
		/// Register with CallManager.
		/// </summary>
		/// <param name="macAddress">Client's MAC address.</param>
		/// <param name="clientAddress">Client's address.</param>
		/// <param name="callManagerAddresses">IPEndPoint addresses of
		/// CallManagers (primary, secondary, etc.).</param>
		/// <param name="deviceType">Device type, e.g., Station Telecaster
		/// Manager.</param>
		/// <param name="srstAddresses">List of SRST addresses.</param>
		/// <param name="tftpAddress">Address of TFTP server.</param>
		/// <param name="version">Version of client to report to
		/// CallManager.</param>
		public OpenDeviceRequest(string macAddress, IPAddress clientAddress,
			ArrayList callManagerAddresses, ArrayList srstAddresses,
			IPEndPoint tftpAddress,
			DeviceType deviceType, ProtocolVersion version)
		{
			this.macAddress = macAddress;
			this.clientAddress = clientAddress;
			this.callManagerAddresses = callManagerAddresses;
			this.srstAddresses = srstAddresses;
			this.tftpAddress = tftpAddress;
			this.deviceType = deviceType;
			this.version = version;
		}

		/// <summary>
		/// Property whose value is whether SRST is enabled, which is
		/// determined by whether any SRST servers are defined.
		/// </summary>
		public bool SrstEnabled {
			get { return srstAddresses != null && srstAddresses.Count > 0; } }

		/// <summary>
		/// MAC address of device requesting registration with the CallManager.
		/// </summary>
		/// <remarks>
		/// This is NOT the sid (a.k.a, Skinny identifier and device name).
		/// This is a string that contains a 12-digit hexadecimal number that
		/// represents the device's MAC address.
		/// </remarks>
		public string macAddress;

		/// <summary>
		/// IPAddress of the device requesting registration.
		/// </summary>
		public IPAddress clientAddress;

		/// <summary>
		/// List of CallManager IPEndPoints for which this device is requesting
		/// registration.
		/// </summary>
		/// <remarks>At any one time, a device is registered with a single CallManager.</remarks>
		public ArrayList callManagerAddresses;

		/// <summary>
		/// List of SRST IPEndPoints which this device may use for recovery.
		/// </summary>
		/// <remarks>(SRST stands for Survivable Remote Site Telephony.)</remarks>
		public ArrayList srstAddresses;

		/// <summary>
		/// List of TFTP IPEndPoints which this device may use.
		/// </summary>
		/// <remarks>(TFTP stands for Trivial File Transfer Protocol.)</remarks>
		public IPEndPoint tftpAddress;

		/// <summary>
		/// Type of the SCCP device requesting registration.
		/// </summary>
		public DeviceType deviceType;

		/// <summary>
		/// Version of the SCCP protocol that this device is using.
		/// </summary>
		/// <remarks>I suppose a device may support multiple versions of the
		/// protocol, so this is not a fixed property of the device.</remarks>
		public ProtocolVersion version;
	}

	/// <summary>
	/// Notification that call is proceeding (it hasn't been rejected or
	/// accepted yet).
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class Proceeding : ClientMessage
	{
		/// <summary>
		/// Call is proceeding on line 1 (don't give up yet).
		/// </summary>
		public Proceeding() : this(1) { }

		/// <summary>
		/// Call is proceeding (don't give up yet).
		/// </summary>
		/// <param name="lineNumber">Number of line connected.</param>
		public Proceeding(uint lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Client is now registered with CallManager.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class Registered : ClientMessage
	{
		/// <summary>
		/// Client is now registered with CallManager.
		/// </summary>
		/// <remarks>
		/// Consumer must subsequently provide relevant information.
		/// </remarks>
		public Registered() : this(new ArrayList()) { }

		/// <summary>
		/// Client is now registered with CallManager.
		/// </summary>
		/// <param name="lines">Array of information about each line.</param>
		public Registered(ArrayList lines)
		{
			this.lines = lines;
		}

		/// <summary>
		/// Information about each Line that this device has available for this
		/// registration.
		/// </summary>
		public ArrayList lines;
	}

	/// <summary>
	/// Hangup. Call has been terminated.
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class Release : ClientMessage
	{
		/// <summary>
		/// Terminate call on default line number of 1.
		/// </summary>
		public Release() : this(1) { }

		/// <summary>
		/// Terminate call.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public Release(uint lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Acknowledgement to terminating client that other client knows call has been terminated.
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class ReleaseComplete : ClientMessage
	{
		/// <summary>
		/// Acknowledge call termination on default line number of 1 with normal cause.
		/// </summary>
		public ReleaseComplete() : this(1) { }

		/// <summary>
		/// Acknowledge call termination with normal cause.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public ReleaseComplete(uint lineNumber) : this(lineNumber, Cause.Normal) { }

		/// <summary>
		/// Acknowledge call termination.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="cause">Cause of this ReleaseComplete message.</param>
		public ReleaseComplete(uint lineNumber, Cause cause)
		{
			this.lineNumber = lineNumber;
			this.cause = cause;
		}

		/// <summary>
		/// Reason that this message was generated by the stack.
		/// </summary>
		public enum Cause
		{
			Normal,			// Call terminating normally.
			NotConnected,	// Device is not connected to a CallManager.
			NotRegistered,	// Device is not registered with a Callmanager.
			CallIdInUse,	// Attempt to place outgoing call using id of existing call.
			NoCallOnLine,	// There was no Call on the specified line.
		}

		/// <summary>
		/// Cause of this message.
		/// </summary>
		public Cause cause;

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Initiate call (make or answer call).
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class Setup : ClientMessage
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public Setup() : this(string.Empty) { }

		/// <summary>
		/// Initiate call on line 1.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="calledPartyNumber">Called-party number.</param>
		public Setup(string calledPartyNumber) : this(1, calledPartyNumber) { }

		/// <summary>
		/// Initiate call.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="calledPartyNumber">Called-party number.</param>
		public Setup(uint lineNumber, string calledPartyNumber) :
			this(lineNumber, calledPartyNumber, string.Empty, string.Empty, string.Empty) { }

		/// <summary>
		/// Receive call.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="calledPartyNumber">Called-party number.</param>
		/// <param name="originalCalledPartyNumber">Original called-party number.</param>
		/// <param name="callingPartyNumber">Calling-party number.</param>
		/// <param name="callingPartyName">Calling-party name.</param>
		public Setup(uint lineNumber,
			string calledPartyNumber, string originalCalledPartyNumber,
			string callingPartyNumber, string callingPartyName)
		{
			this.lineNumber = lineNumber;
			this.calledPartyNumber = calledPartyNumber;
			this.originalCalledPartyNumber = originalCalledPartyNumber;
			this.callingPartyNumber = callingPartyNumber;
			this.callingPartyName = callingPartyName;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Called-party number.
		/// </summary>
		public string calledPartyNumber;

		/// <summary>
		/// Original called-party number in case, for example, call has been forwarded.
		/// </summary>
		public string originalCalledPartyNumber;

		/// <summary>
		/// Calling-party number.
		/// </summary>
		public string callingPartyNumber;

		/// <summary>
		/// Calling-party name, a.k.a., display name.
		/// </summary>
		public string callingPartyName;
	}

	/// <summary>
	/// Acknowledges a call initiation.
	/// </summary>
	/// <remarks>Receive and send.</remarks>
	public class SetupAck : ClientMessage
	{
		/// <summary>
		/// Acknowledge receipt of Setup, using default line number of 1.
		/// </summary>
		public SetupAck() : this(1) { }

		/// <summary>
		/// Acknowledge receipt of Setup.
		/// </summary>
		/// <param name="lineNumber">Line number on which Setup was received.</param>
		public SetupAck(uint lineNumber)
		{
			this.lineNumber = lineNumber;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }
	}

	/// <summary>
	/// Client can start sending media.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class StartTransmit : ClientMessage
	{
		/// <summary>
		/// Start sending media, using default line number of 1.
		/// </summary>
		/// <param name="media">Media information.</param>
		public StartTransmit(MediaInfo media) : this(1, media) { }

		/// <summary>
		/// Start sending media.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		/// <param name="media">Media information.</param>
		public StartTransmit(uint lineNumber, MediaInfo media)
		{
			this.lineNumber = lineNumber;
			this.media = media;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Describes media that this message is telling the device to send.
		/// </summary>
		public MediaInfo media;

	}

	/// <summary>
	/// Client must stop sending media.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class StopTransmit : ClientMessage
	{
		/// <summary>
		/// Stop sending media, using all defaults.
		/// </summary>
		public StopTransmit() : this(0, 0) { }

		/// <summary>
		/// Stop sending media, using default line number of 1.
		/// </summary>
		/// <param name="conferenceId">Conference identifier.</param>
		/// <param name="passthruPartyId">Pass-thru party identifier.</param>
		public StopTransmit(uint conferenceId,
			uint passthruPartyId) : this(1, conferenceId, passthruPartyId) { }

		/// <summary>
		/// Stop sending media.
		/// </summary>
		/// <param name="lineNumber">Line number.</param>
		public StopTransmit(uint lineNumber, uint conferenceId,
			uint passthruPartyId)
		{
			this.lineNumber = lineNumber;
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
		}

		/// <summary>
		/// Line number.
		/// </summary>
		public uint lineNumber;

		/// <summary>
		/// Property whose value is the line number.
		/// </summary>
		public override uint Line { get { return lineNumber; } }

		/// <summary>
		/// Conference identifier. Identifies messages belonging to a
		/// particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier. Typically ties a response to a request so
		/// that the receiver of a response knows to which request a message is
		/// in response.
		/// </summary>
		public uint passthruPartyId;
	}

	/// <summary>
	/// Client is now unregistered with CallManager.
	/// </summary>
	/// <remarks>Receive only.</remarks>
	public class Unregistered : ClientMessage
	{
		/// <summary>
		/// Client is now unregistered with CallManager.
		/// </summary>
		public Unregistered() { }
	}

	#region Major objects used as parameters to client messages
	/// <summary>
	/// Connection information, i.e., names and numbers associated with call.
	/// </summary>
	public class ConnInfo
	{
		/// <summary>
		/// Constructor for later assignment of fields.
		/// </summary>
		public ConnInfo() { }

		/// <summary>
		/// Constructor taking all fields as parameters.
		/// </summary>
		/// <param name="callingPartyName">Name of the calling party.</param>
		/// <param name="callingPartyNumber">Directory number of the calling party.</param>
		/// <param name="calledPartyName">Name of the called party.</param>
		/// <param name="calledPartyNumber">Directory number of the called party.</param>
		/// <param name="originalCalledPartyName">Name of the original called
		/// party for a call that has been forwarded.</param>
		/// <param name="originalCalledPartyNumber">Directory number of the
		/// original called party for a call that has been forwarded.</param>
		/// <param name="lastRedirectingPartyName">Name of the last redirecting
		/// party for a call that has been forwarded.</param>
		/// <param name="lastRedirectingPartyNumber">Directory number of the
		/// original called party for a call that has been forwarded.</param>
		public ConnInfo(string callingPartyName, string callingPartyNumber,
			string calledPartyName, string calledPartyNumber,
			string originalCalledPartyName, string originalCalledPartyNumber,
			string lastRedirectingPartyName, string lastRedirectingPartyNumber)
		{
			this.callingPartyName = callingPartyName;
			this.callingPartyNumber = callingPartyNumber;
			this.calledPartyName = calledPartyName;
			this.calledPartyNumber = calledPartyNumber;
			this.originalCalledPartyName = originalCalledPartyName;
			this.originalCalledPartyNumber = originalCalledPartyNumber;
			this.lastRedirectingPartyName = lastRedirectingPartyName;
			this.lastRedirectingPartyNumber = lastRedirectingPartyNumber;
		}

		/// <summary>
		/// Name of the calling party.
		/// </summary>
		public string callingPartyName;

		/// <summary>
		/// Directory number of the calling party.
		/// </summary>
		public string callingPartyNumber;

		/// <summary>
		/// Name of the called party.
		/// </summary>
		public string calledPartyName;

		/// <summary>
		/// Directory number of the called party.
		/// </summary>
		public string calledPartyNumber;

		/// <summary>
		/// Name of the original called party for a call that has been
		/// forwarded.
		/// </summary>
		public string originalCalledPartyName;

		/// <summary>
		/// Directory number of the original called party for a call that has
		/// been forwarded.
		/// </summary>
		public string originalCalledPartyNumber;

		/// <summary>
		/// Name of the last redirecting party for a call that has been
		/// forwarded.
		/// </summary>
		public string lastRedirectingPartyName;

		/// <summary>
		/// Directory number of the last redirecting party for a call that has
		/// been forwarded.
		/// </summary>
		public string lastRedirectingPartyNumber;
	}

	/// <summary>
	/// Media information, e.g., payload type.
	/// </summary>
	public class MediaInfo
	{
		/// <summary>
		/// Constructor for later assignment of fields.
		/// </summary>
		public MediaInfo() { }

		/// <summary>
		/// Constructor taking all fields as parameters except for G.723.1 bit
		/// rate, which defaults to high-rate if G.723.1 is the payload type.
		/// </summary>
		/// <param name="address">IPEndPoint address of the device receiving
		/// the RTP media stream.</param>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="packetSize">Number of milliseconds of media that an
		/// RTP packet contains.</param>
		/// <param name="payloadType">Type of the data contained in the payload
		/// portion of the RTP packet.</param>
		/// <param name="echoCancellation">Whether echo cancellation is
		/// enabled.</param>
		/// <param name="precedence">Precedence of the RTP stream.</param>
		/// <param name="silenceSuppression">Whether silence suppression is
		/// enabled.</param>
		/// <param name="maxFramesPerPacket">Most frames allowed in an RTP
		/// packet.</param>
		public MediaInfo(IPEndPoint address, uint conferenceId,
			uint passthruPartyId, uint packetSize, PayloadType payloadType,
			bool echoCancellation, uint precedence,
			bool silenceSuppression, uint maxFramesPerPacket)
			: this(address, conferenceId, passthruPartyId, packetSize, payloadType,
			payloadType == PayloadType.G7231 ? G723BitRate._5_3khz : G723BitRate.NotApplicable,
			echoCancellation, precedence, silenceSuppression, maxFramesPerPacket) { }

		/// <summary>
		/// Constructor taking all fields as parameters.
		/// </summary>
		/// <param name="address">IPEndPoint address of the device receiving
		/// the RTP media stream.</param>
		/// <param name="conferenceId">Identifies messages belonging to a
		/// particular conference.</param>
		/// <param name="passthruPartyId">Typically ties a response to a
		/// request so that the receiver of a response knows to which request a
		/// message is in response.</param>
		/// <param name="packetSize">Number of milliseconds of media that an
		/// RTP packet contains.</param>
		/// <param name="payloadType">Type of the data contained in the payload
		/// portion of the RTP packet.</param>
		/// <param name="g723BitRate">For G.723.1 only, this is the bit rate
		/// being used.</param>
		/// <param name="echoCancellation">Whether echo cancellation is
		/// enabled.</param>
		/// <param name="precedence">Precedence of the RTP stream.</param>
		/// <param name="silenceSuppression">Whether silence suppression is
		/// enabled.</param>
		/// <param name="maxFramesPerPacket">Most frames allowed in an RTP
		/// packet.</param>
		public MediaInfo(IPEndPoint address, uint conferenceId,
			uint passthruPartyId, uint packetSize, PayloadType payloadType,
			G723BitRate g723BitRate, bool echoCancellation, uint precedence,
			bool silenceSuppression, uint maxFramesPerPacket)
		{
			this.address = address;
			this.conferenceId = conferenceId;
			this.passthruPartyId = passthruPartyId;
			this.packetSize = packetSize;
			this.payloadType = payloadType;
			this.echoCancellation = echoCancellation;
			this.g723BitRate = g723BitRate;
			this.precedence = precedence;
			this.silenceSuppression = silenceSuppression;
			this.maxFramesPerPacket = maxFramesPerPacket;
		}

		/// <summary>
		/// IPEndPoint address of the device receiving the RTP media stream.
		/// </summary>
		public IPEndPoint address;

		/// <summary>
		/// Identifies messages belonging to a particular conference.
		/// </summary>
		public uint conferenceId;

		/// <summary>
		/// Transaction identifier.
		/// </summary>
		/// <remarks>
		/// Typically ties a response to a request so that the receiver of a
		/// response knows to which request a message is in response.
		/// </remarks>
		public uint passthruPartyId;

		/// <summary>
		/// Number of milliseconds of media that an RTP packet contains.
		/// </summary>
		public uint packetSize;

		/// <summary>
		/// Type of the data contained in the payload portion of the RTP
		/// packet.
		/// </summary>
		public PayloadType payloadType;

		/// <summary>
		/// Whether echo cancellation is enabled.
		/// </summary>
		public bool echoCancellation;

		/// <summary>
		/// For G.723.1 only, this is the bit rate being used.
		/// </summary>
		public G723BitRate g723BitRate;

		/// <summary>
		/// Precedence of the RTP stream.
		/// </summary>
		public uint precedence;

		/// <summary>
		/// Whether silence suppression is enabled.
		/// </summary>
		public bool silenceSuppression;

		/// <summary>
		/// Most frames allowed in an RTP packet.
		/// </summary>
		public uint maxFramesPerPacket;
	}
	#endregion
}
