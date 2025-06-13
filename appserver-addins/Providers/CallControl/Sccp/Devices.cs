using System;
using System.Net;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Core.IPC;
using Metreos.Utilities;
using Metreos.Utilities.Collections;
using Metreos.SccpStack;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.CallControl.Sccp;
using Metreos.Messaging.MediaCaps;

namespace Metreos.CallControl.Sccp
{
	/// <summary>
	/// Represents an encapsulation of a collection that contain entries
	/// for all devices, indexed by MAC address.
	/// </summary>
	public class Devices : IEnumerable
	{
		/// <summary>
		/// Constructs a Devices object.
		/// </summary>
		/// <param name="provider">SccpProvider.</param>
		/// <param name="configUtility">For accessing configuration
		/// data.</param>
		/// <param name="log">Where diagnostics are written to.</param>
		/// <param name="registeredDevices">Collection of Devices that are
		/// registered with a CallManager.</param>
		/// <param name="engagedDevices">Collection of Devices that are
		/// engaged in a Call.</param>
		/// <param name="recentlyEngagedDevices">Fixed-size collection of Device MAC
		/// addresses for the last N terminated calls.</param>
		/// <param name="registrationThrottle">Controls the sending of
		/// OpenDeviceRequest and CloseDeviceRequest messages to the stack so
		/// that the CallManager is never overwhelmed.</param>
		public Devices(SccpProvider provider, IConfigUtility configUtility, LogWriter log,
			RegisteredDevices registeredDevices, EngagedDevices engagedDevices,
			BoundedCollection recentlyEngagedDevices,
			ProcessObject registrationThrottle)
		{
			Assertion.Check(configUtility != null,
				"SccpStack: cannot create Devices with null configUtility");

			this.provider = provider;
			this.log = log;
			this.configUtility = configUtility;
			this.registeredDevices = registeredDevices;
			this.engagedDevices = engagedDevices;
			this.recentlyEngagedDevices = recentlyEngagedDevices;
			this.registrationThrottle = registrationThrottle;

			devices = Hashtable.Synchronized(new Hashtable());
		}

		/// <summary>
		/// The Hashtable that contains entries for all devices, indexed by
		/// MAC address.
		/// </summary>
        private Hashtable devices;

		/// <summary>
		/// For accessing configuration data.
		/// </summary>
        private IConfigUtility configUtility;

		/// <summary>
		/// SccpProvider.
		/// </summary>
		protected readonly SccpProvider provider;

		/// <summary>
		/// Where diagnostics are written to.
		/// </summary>
		private LogWriter log;

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

        #region Collection accessors

		/// <summary>
		/// Creates an SCCP Device and adds it to the device collection.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="newDevice">An SCCP device for this device to
		/// communicate with its CallManagers.</param>
        public void CreateDevice(string macAddress, SccpStack.Device newDevice)
        {
			if (checkMacAddress(macAddress, false) && newDevice != null)
			{
				// If there is already a device with this MAC address, remove
				// it from the devices Hashtable and make sure it is
				// unregistered.
				Unregister(macAddress, false);

				Device old = AddDevice(macAddress,
					new Device(provider, macAddress, newDevice, configUtility, log,
					this, registeredDevices, engagedDevices,
					recentlyEngagedDevices, registrationThrottle));

				Assertion.Check(old == null,
					"SccpStack: creating Device with same MAC address as existing device");
			}
			else
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: CreateDevice failed, macAddress {1}, device {2}",
					macAddress, checkMacAddress(macAddress, false), newDevice != null);
			}
        }

		/// <summary>
		/// Returns whether there is a Device that has the specified device
		/// name.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <returns>Whether there is a Device that has the specified device
		/// name.</returns>
        public bool ContainsName(string macAddress)
        {
            return devices.Contains(macAddress);
        }

		/// <summary>
		/// Gets the Device that has the specified MAC address.
		/// </summary>
		public Device this [ string macAddress ]
		{
			get
			{
				checkMacAddress(macAddress, true);
				return devices[macAddress] as Device;
			}
		}

		/// <summary>
		/// Returns whether the specified MAC address is minimally valid. If
		/// not and toss is true, throw an exception.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="toss">Whether to throw an exception if the MAC address
		/// is not valid.</param>
		/// <returns>Whether the specified MAC address is minimally
		/// valid</returns>
		private bool checkMacAddress(string macAddress, bool toss)
		{
			bool valid = macAddress != null && macAddress.Length > 0;

			if (!valid && toss)
			{
				throw new NullReferenceException(
					"macAddress == null || macAddress.Length == 0");
			}

			return valid;
		}

		/// <summary>
		/// Returns the call id of the active call on the device with the
		/// specified MAC address.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <returns>The call id of the active call on the device with the
		/// specified MAC address. If no such device or no active call, return
		/// 0.</returns>
        public long GetCallId(string macAddress)
        {
            Device device = this[macAddress];

			return device != null ? device.GetCallId() : 0;
        }

		/// <summary>
		/// Registers the Device with the specified MAC address according to
		/// the specified CallManager addresses.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="callManagerIPAddresses">CallManager addresses
		/// (primary, secondary, etc.).</param>
		/// <param name="srstAddresses">List of SRST addresses.</param>
		/// <param name="callManagerPort">Port on which CallManagers are
		/// listening for registrations.</param>
		public void Register(string macAddress, IPAddress[] callManagerIPAddresses,
			IPAddress[] srstAddresses, int callManagerPort)
		{
			Device device = this[macAddress];
			if (device != null)
			{
				device.Register(callManagerIPAddresses, srstAddresses,
					callManagerPort);
			}
		}

		/// <summary>
		/// Removes and unregisters the Device with the specified MAC address.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="expedite">Whether to expedite unregistering this
		/// device, i.e., not waiting for a response from the CM.</param>
		public void Unregister(string macAddress, bool expedite)
		{
			Device device = RemoveDevice(macAddress);
			if (device != null)
			{
				device.Unregister(expedite);
			}
		}

		/// <summary>
		/// Adds a Device to the collection by MAC address.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="dInfo">Device to add.</param>
		/// <returns>Device previously stored in collection with that name;
		/// null, otherwise.</returns>
		public Device AddDevice(string macAddress, Device device)
		{
			checkMacAddress(macAddress, true);
			Device old;
			lock (devices.SyncRoot)
			{
				old = (Device)devices[macAddress];
				devices[macAddress] = device;
			}
			return old;
		}

		/// <summary>
		/// Removes Device from collection that has specified MAC address.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <returns>Device removed from collection or null if no device with
		/// that MAC address.</returns>
        private Device RemoveDevice(string macAddress)
		{
			checkMacAddress(macAddress, true);
			Device device;
			lock (devices.SyncRoot)
			{
				device = this[macAddress];
				if (device != null)
				{
					devices.Remove(macAddress);
				}
			}
			return device;
		}

		/// <summary>
		/// Removes every Device from the devices collection and
		/// unregisters them.
		/// </summary>
		/// <param name="expedite">Whether to expedite unregistering
		/// devices, i.e., not waiting for responses from the CM.</param>
		public void Clear(bool expedite)
        {
			Device[] list;

            lock (devices.SyncRoot)
            {
				list = new Device[devices.Values.Count];
				devices.Values.CopyTo(list, 0);
				devices.Clear();
			}

			foreach (Device device in list)
			{
				device.Unregister(expedite);
			}
        }

        #endregion

        #region CallControl methods

		/// <summary>
		/// Finds Device with the specified call id and triggers an
		/// Alerting event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="message">Alerting message from the SCCP stack.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ReceivedAlerting(long callId, Alerting message)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.ReceivedAlerting, message);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// ReceivedConnect event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="message">Connect message from the SCCP stack.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ReceivedConnect(long callId, Connect message)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.ReceivedConnect, message);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// ReceivedConnectAck event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ReceivedConnectAck(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.ReceivedConnectAck);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// SetMediaEvent event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="mediaAddr">RTP (UDP) IPEndPoint address.</param>
		/// <param name="codec">Codec.</param>
		/// <param name="framesize">Frame size on milliseconds.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool SetMedia(long callId, IPEndPoint mediaAddr,
			IMediaControl.Codecs codec, uint framesize)
        {
            Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.SetMedia,
				new SetMediaEvent(mediaAddr));
        }

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// UseMohMedia (Music-On-Hold) event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool UseMohMedia(long callId)
		{
			Device device = engagedDevices[callId];
			//it is possible to get a request after the call has terminiated
			//because we responds to StopTransmit before we got ReceiveRelease
			//and we don't know whether StopTransmit is triggered due to a hold
			//or end call request. In order to get rid of a meaningless warning,
			//we have to treat device==null as a successful handling of UseMohMedia
			//request.
			return device == null ||
				device.CallTrigger((int)Call.EventType.UseMohMedia);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// ClearMedia event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ClearMedia(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.ClearMedia);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// Hold event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool HandleHold(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.Hold);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// Resume event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="rxIP">receiving RTP IP</param>
		/// <param name="rxPort">receiving RTP port</param>
		/// <param name="rxControlIP">receiving RCTP IP</param>
		/// <param name="rxControlPort">receiving RCTP port</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool HandleResume(long callId, string rxIP, uint rxPort,
			string rxControlIP, uint rxControlPort)
		{
			Device device = engagedDevices[callId];

			IPEndPoint mediaAddr = null;
			if (rxIP != null)
				mediaAddr = new IPEndPoint(IPAddress.Parse(rxIP), (int)rxPort);
			return device != null &&
				device.CallTrigger((int)Call.EventType.Resume, new SetMediaEvent(mediaAddr));
		}

		public bool HandleMohDisabled(long callId)
		{
			Device device = engagedDevices[callId];

			return device != null &&
				device.CallTrigger((int)Call.EventType.MohDisabled);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// StartTransmit event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="message">StartTransmit message from the SCCP
		/// stack.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool StartTransmit(long callId, ClientMessage message)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.StartTransmit, message);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a StopTransmit
		/// event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool StopTransmit(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.StopTransmit);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers an Accept
		/// event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool AcceptCall(long callId)
        {
            Device device = engagedDevices[callId];
            return device != null &&
				device.CallTrigger((int)Call.EventType.Accept);
        }

		/// <summary>
		/// Finds Device with the specified call id and triggers an Answer
		/// event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool AnswerCall(long callId)
        {
            Device device = engagedDevices[callId];
            return device != null &&
				device.CallTrigger((int)Call.EventType.Answer);
        }

		/// <summary>
		/// Finds Device with the specified call id and triggers a Redirect
		/// event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns></returns>
		public bool RedirectCall(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null && device.RedirectCall();
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a Reject event
		/// on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns></returns>
		public bool RejectCall(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null && device.RejectCall();
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// SendUserInput (DTMF) event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="digits"></param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool SendUserInput(long callId, string digits)
        {
            Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.SendUserInput,
				new Digits(digits));
        }

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// Hangup event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns></returns>
        public bool HangupCall(long callId)
        {
            Device device = engagedDevices[callId];
			return device != null && device.HangupCall();
        }

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// ReceivedRelease event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="releaseMessage">Release message.</param>
		/// <returns></returns>
        public bool ReceivedRelease(long callId, Release releaseMessage)
        {
            Device device = engagedDevices[callId];
			return device != null && device.ReceivedRelease(releaseMessage);
        }

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// ReceivedReleaseComplete event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="releaseCompleteMessage">ReleaseComplete message.</param>
		/// <returns></returns>
		public bool ReceivedReleaseComplete(long callId,
			ReleaseComplete releaseCompleteMessage)
		{
			Device device = engagedDevices[callId];
			return device != null && device.ReceivedReleaseComplete(releaseCompleteMessage);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers an
		/// OpenReceiveRequest event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="message">OpenReceiveRequest message from the SCCP
		/// stack.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ReceivedOpenReceiveRequest(long callId,
			OpenReceiveRequest message)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.OpenReceiveRequest,
				message);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// ReceivedDigits (DTMF) event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="message"></param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool ReceivedDigits(long callId, Digits message)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.ReceivedDigits,
				message);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// StartHoldTone event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool StartHoldTone(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.StartHoldTone);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers a
		/// StopHoldTone event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool StopTone(long callId)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.StopTone);
		}

		/// <summary>
		/// Finds Device with the specified call id and triggers an
		/// CallInfo event on the attached Call.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="message">CallInfo message from the SCCP
		/// stack.</param>
		/// <returns>Whether the event was successfully triggered.</returns>
		public bool CallInfo(long callId,
			CallInfo message)
		{
			Device device = engagedDevices[callId];
			return device != null &&
				device.CallTrigger((int)Call.EventType.CallInfo,
				message);
		}

        #endregion

        #region Device refresh

		/// <summary>
		/// Mark each Device as not having been checked yet for whether it
		/// needs to be registered.
		/// </summary>
        public void ClearConfirmationFlags()
        {
            lock (devices.SyncRoot)
            {
                foreach (Device device in devices.Values)
                {
                    device.Confirmed = false;
                }
            }
        }

		/// <summary>
		/// Mark the Device with the specified MAC address as having been
		/// confirmed to be registered.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		public void Confirm(string macAddress)
        {
            Device device = this[macAddress];
			if (device != null)
			{
				device.Confirmed = true;
			}
        }

		/// <summary>
		/// Unregister devices that were registered but are no longer present
		/// in an SCCP device pool.
		/// </summary>
		/// <param name="expedite">Whether to expedite unregistering
		/// devices, i.e., not waiting for responses from the CM.</param>
		public void ClearUnconfirmedDevices(bool expedite)
        {
            lock (devices.SyncRoot)
            {
                StringCollection defunctDevs = new StringCollection();

                foreach (DictionaryEntry de in devices)
                {
                    string macAddress = de.Key as String;
                    Device device = de.Value as Device;

                    if (!device.Confirmed)
                    {
                        SccpConnection primaryCCM;
						if (device.HavePrimaryConnection(out primaryCCM))
						{
							log.Write(TraceLevel.Info, "Prv: unregistering: {0}@{1}",
								device.MacAddress, primaryCCM);
						}
						else
						{
							log.Write(TraceLevel.Info, "Prv: unregistering: {0}", device.MacAddress);
						}

                        device.Unregister(expedite);
                        defunctDevs.Add(macAddress);
                    }
                }

                foreach (string macAddress in defunctDevs)
                {
                    devices.Remove(macAddress);
                }
            }
        }

        #endregion

        #region IEnumerable Members

        public object SyncRoot { get { return devices.SyncRoot; } }

        public IEnumerator GetEnumerator()
        {
            return devices.GetEnumerator();
        }

        #endregion
    }
}
