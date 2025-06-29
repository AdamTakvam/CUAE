using System;
using System.Net;
using System.Management;
using System.Diagnostics;
using System.Collections;

using Metreos.Utilities;
using Metreos.LoggingFramework;
using Metreos.Utilities.Selectors;

namespace Metreos.SccpStack
{
	/// <summary>
	/// Represents the SCCP stack.
	/// </summary>
	/// <remarks>
	/// This is the highest-level abstraction in the SccpStack.
	/// It is essentially a low-level, connection factory and high-level,
	/// device factory.
	/// </remarks>
	public class SccpStack
	{
		/// <summary>
		/// Constructs an SccpStack.
		/// </summary>
		/// <param name="log">Object through which log entries are generated.</param>
		public SccpStack(LogWriter log)
		{
            IPAddress myIP = IpUtility.ResolveHostname(Dns.GetHostName());
            if (myIP == null)
                throw new ApplicationException("Cannot determine local IP address");

            myIpAddress = myIP.ToString();
			myMacAddress = GetMacAddress();

			this.log = log;

            log.Write(TraceLevel.Info, "Local network config: IP={0} MAC={1}", myIpAddress, myMacAddress);

			deviceIdFactory = new TagFactory();

			threadPool = new ThreadPool(initialSelectorActionThreadpoolSize,
				maxSelectorActionThreadpoolSize, "SCCP selected actions");
            threadPool.MessageLogged += new Metreos.Utilities.LogDelegate(log.Write);
			threadPool.Start();

			selector = StartSelector();
		}

		/// <summary>
		/// Thread pool to offload processing of selected actions from
		/// selector callback.
		/// </summary>
		private readonly ThreadPool threadPool;

		/// <summary>
		/// Stop the SccpStack.
		/// </summary>
		public void Stop()
		{
			selector.Stop();
		}

		/// <summary>
		/// "Global" constant for how often to check for lock. 
		/// </summary>
		internal const int LockPollMs = 1 * 1000;

		/// <summary>
		/// Generates unique device identifiers across all devices in the
		/// SccpStack.
		/// </summary>
		private readonly TagFactory deviceIdFactory;

		/// <summary>
		/// Wrapper for the thread doing Socket.Select().
		/// </summary>
		/// <remarks>
		/// All Socket operations are performed through this object.
		/// 
		/// The underlying class provides its own access control.
		/// </remarks>
		private readonly SelectorBase selector;

		/// <summary>
		/// Object through which log entries are generated.
		/// </summary>
		private LogWriter log;

        /// <summary>
        /// Constructs then starts SuperSelector.
        /// </summary>
        /// <returns>Initialized SuperSelector.</returns>
        internal SelectorBase StartSelector()
        {
            SelectorBase selector = new SuperSelector(
                new Utilities.Selectors.SelectedDelegate(SccpConnection.Selected),
                new Utilities.Selectors.SelectedExceptionDelegate(SccpConnection.SelectedException),
                new Utilities.Selectors.LogDelegate(LogMessage));
            selector.Start();

            return selector;
        }

        public void LogMessage(TraceLevel level, string msg, Exception e)
        {
            if(e == null)
                log.Write(level, msg);
            else
                log.Write(level, "{0} - {1}", msg, e.Message);
        }

		/// <summary>
		/// Creates a low-level, SccpConnection.
		/// </summary>
		/// <remarks>
		/// All SccpConnections use the same Selector.
		/// </remarks>
		/// <param name="address">IPEndPoint address of CallManager to which we
		/// connect.</param>
		/// <returns>An SccpConnection object.</returns>
		public SccpConnection CreateConnection(IPEndPoint address)
		{
			return new SccpConnection(log, address, selector, threadPool);
		}

		/// <summary>
		/// Creates a high-level, Device.
		/// </summary>
		/// <remarks>
		/// All Devices use the same Selector and MediaCapability.
		/// </remarks>
		/// <returns>An Device object.</returns>
		public Device CreateDevice()
		{
			return new Device(log, selector,
				deviceIdFactory.Next, threadPool);
		}
		/// <summary>
		/// Initial threads in the Selector-action thread pool.
		/// </summary>
		private static int initialSelectorActionThreadpoolSize = 5;

		/// <summary>
		/// Maximum number of threads in the Selector-action thread pool.
		/// </summary>
		private static int maxSelectorActionThreadpoolSize = 15;

		#region Selector-action configuration parameters
		public int InitialSelectorActionThreadpoolSize { set { initialSelectorActionThreadpoolSize = value; } }
		public int MaxSelectorActionThreadpoolSize { set { maxSelectorActionThreadpoolSize = value; } }
		#endregion

		#region EventTimer configuration parameters
		public int InitialTimerThreadpoolSize { set { EventTimer.InitialTimerThreadpoolSize = value; } }
		public int MaxTimerThreadpoolSize { set { EventTimer.MaxTimerThreadpoolSize = value; } }
		#endregion

		#region SccpConnection configuration parameters
		public int LingerSec { set { SccpConnection.LingerSec = value; } }
		#endregion

		#region CallManager configuration parameters
		public int MinWaitForKeepaliveAfterRegisterSec { set { CallManager.MinWaitForKeepaliveAfterRegisterSec = value; } }
		public int MaxWaitForKeepaliveAfterRegisterSec { set { CallManager.MaxWaitForKeepaliveAfterRegisterSec = value; } }
		public int WaitingForRegisterAckKeepaliveMs { set { CallManager.WaitingForRegisterAckKeepaliveMs = value; } }
		public int RetryTokenRequestMs { set { CallManager.RetryTokenRequestMs = value; } }
		public int StuckAlarmCount { set { CallManager.StuckAlarmCount = value; } }
		public int CallManagerConnectRetries { set { CallManager.CallManagerConnectRetries = value; } }
		public int SrstConnectRetries { set { CallManager.SrstConnectRetries = value; } }
		public int KeepaliveJitterPercent { set { CallManager.KeepaliveJitterPercent = value; } }
		#endregion

		#region Discovery configuration parameters
		public int CallEndMs { set { Discovery.CallEndMs = value; } }
		public int CloseMs { set { Discovery.CloseMs = value; } }
		public int DevicePollMs { set { Discovery.DevicePollMs = value; } }
		public int DevicePollJitterPercent { set { Discovery.DevicePollJitterPercent = value; } }
		public int LockoutMs { set { Discovery.LockoutMs = value; } }
		public int NakToSynRetryMs { set { Discovery.NakToSynRetryMs = value; } }
		public int ConnectMs { set { Discovery.ConnectMs = value; } }
		public int DefaultKeepaliveMs { set { Discovery.DefaultKeepaliveMs = value; } }
		public int PadMs { set { Discovery.PadMs = value; } }
		public int WaitingForUnregisterMs { set { Discovery.WaitingForUnregisterMs = value; } }
		public int NoCallManagersDefinedMs { set { Discovery.NoCallManagersDefinedMs = value; } }
		public int RetryPrimaryMs { set { Discovery.RetryPrimaryMs = value; } }
		public int MaxCallManagerListIterations { set { Discovery.MaxCallManagerListIterations = value; } }
		public int MinWaitForUnregisterSec { set { Discovery.MinWaitForUnregisterSec = value; } }
		public int MaxWaitForUnregisterSec { set { Discovery.MaxWaitForUnregisterSec = value; } }
		public int RejectMs { set { Discovery.RejectMs = value; } }
		public string DeviceType_ { set { Discovery.DeviceType_ = ConvertDevice(value); } }	// Underscore differentiates from property.

		public string Version { set { Discovery.Version = ConvertVersion(value); } }
		public int AckRetriesBeforeLockout { set { Discovery.AckRetriesBeforeLockout = value; } }
		public int KeepaliveTimeoutsBeforeNoTokenRegister { set { Discovery.KeepaliveTimeoutsBeforeNoTokenRegister = value; } }
		public int UnregisterAckRetries { set { Discovery.UnregisterAckRetries = value; } }
		#endregion

		#region Registration configuration parameters
		public int MaxLines { set { Registration.MaxLines = value; } }
		public int MaxFeatures { set { Registration.MaxFeatures = value; } }
		public int MaxServiceUrls { set { Registration.MaxServiceUrls = value; } }
		public int MaxSoftkeyDefinitions { set { Registration.MaxSoftkeyDefinitions = value; } }
		public int MaxSoftkeySetDefinitions { set { Registration.MaxSoftkeySetDefinitions = value; } }
		public int MaxSpeeddials { set { Registration.MaxSpeeddials = value; } }
		public ArrayList Codecs { get { return Registration.Codecs; } }
		#endregion

		#region Verbose logging configuration parameters
		public bool LogCallVerbose { set { Call.IsLogVerbose = value; CallCollection.IsLogVerbose = value; } }
		public bool LogCallManagerVerbose { set { CallManager.IsLogVerbose = value; CallManagerCollection.IsLogVerbose = value; } }
		public bool LogConnectionVerbose { set { SccpConnection.IsLogVerbose = value; SccpConnectionFactory.IsLogVerbose = value; } }
		public bool LogDiscoveryVerbose { set { Discovery.IsLogVerbose = value; } }
		public bool LogRegistrationVerbose { set { Registration.IsLogVerbose = value; } }
		public bool LogSystemVerbose { set { EventTimer.IsLogVerbose = value; Device.IsLogVerbose = value; } }
		#endregion

		/// <summary>
		/// Converts from the name of a device type to the corresponding enumerated value.
		/// </summary>
		/// <param name="deviceTypeName">Name of a device type.</param>
		/// <returns>The corresponding enumerated value.</returns>
		private static DeviceType ConvertDevice(string deviceTypeName)
		{
			DeviceType deviceType;

			switch (deviceTypeName)
			{
				default:
				case "StationTelecasterMgr":
					deviceType = DeviceType.StationTelecasterMgr;
					break;

				case "Station30spplus":
					deviceType = DeviceType.Station30spplus;
					break;

				case "Station12spplus":
					deviceType = DeviceType.Station12spplus;
					break;

				case "Station12sp":
					deviceType = DeviceType.Station12sp;
					break;

				case "Station12":
					deviceType = DeviceType.Station12;
					break;

				case "Station30vip":
					deviceType = DeviceType.Station30vip;
					break;

				case "StationTelecaster":
					deviceType = DeviceType.StationTelecaster;
					break;

				case "StationTelecasterBus":
					deviceType = DeviceType.StationTelecasterBus;
					break;

				case "StationPolycom":
					deviceType = DeviceType.StationPolycom;
					break;

				case "Station130spplus":
					deviceType = DeviceType.Station130spplus;
					break;

				case "StationPhoneApplication":
					deviceType = DeviceType.StationPhoneApplication;
					break;

				case "AnalogAccess":
					deviceType = DeviceType.AnalogAccess;
					break;

				case "DigitalAccessTitan1":
					deviceType = DeviceType.DigitalAccessTitan1;
					break;

				case "DigitalAccessTitan2":
					deviceType = DeviceType.DigitalAccessTitan2;
					break;

				case "DigitalAccessLennon":
					deviceType = DeviceType.DigitalAccessLennon;
					break;

				case "AnalogAccessElvis":
					deviceType = DeviceType.AnalogAccessElvis;
					break;

				case "ConferenceBridge":
					deviceType = DeviceType.ConferenceBridge;
					break;

				case "ConferenceBridgeYoko":
					deviceType = DeviceType.ConferenceBridgeYoko;
					break;

				case "H225":
					deviceType = DeviceType.H225;
					break;

				case "H323Phone":
					deviceType = DeviceType.H323Phone;
					break;

				case "H323Trunk":
					deviceType = DeviceType.H323Trunk;
					break;

				case "MusicOnHold":
					deviceType = DeviceType.MusicOnHold;
					break;

				case "Pilot":
					deviceType = DeviceType.Pilot;
					break;

				case "TapiPort":
					deviceType = DeviceType.TapiPort;
					break;

				case "TapiRoutePoint":
					deviceType = DeviceType.TapiRoutePoint;
					break;

				case "VoiceInbox":
					deviceType = DeviceType.VoiceInbox;
					break;

				case "VoiceInboxAdmin":
					deviceType = DeviceType.VoiceInboxAdmin;
					break;

				case "LineAnnunciator":
					deviceType = DeviceType.LineAnnunciator;
					break;

				case "SoftwareMtpDixieland":
					deviceType = DeviceType.SoftwareMtpDixieland;
					break;

				case "CiscoMediaServer":
					deviceType = DeviceType.CiscoMediaServer;
					break;

				case "RouteList":
					deviceType = DeviceType.RouteList;
					break;

				case "LoadSimulator":
					deviceType = DeviceType.LoadSimulator;
					break;

				case "Ipste1":
					deviceType = DeviceType.Ipste1;
					break;

				case "MediaTerminationPoint":
					deviceType = DeviceType.MediaTerminationPoint;
					break;

				case "MediaTerminationPointYoko":
					deviceType = DeviceType.MediaTerminationPointYoko;
					break;

				case "MediaTerminationPointDixieland":
					deviceType = DeviceType.MediaTerminationPointDixieland;
					break;

				case "MediaTerminationPointSummit":
					deviceType = DeviceType.MediaTerminationPointSummit;
					break;

				case "MgcpStation":
					deviceType = DeviceType.MgcpStation;
					break;

				case "MgcpTrunk":
					deviceType = DeviceType.MgcpTrunk;
					break;

				case "RasProxy":
					deviceType = DeviceType.RasProxy;
					break;

				case "Trunk":
					deviceType = DeviceType.Trunk;
					break;

				case "Annunciator":
					deviceType = DeviceType.Annunciator;
					break;

				case "MonitorBridge":
					deviceType = DeviceType.MonitorBridge;
					break;

				case "Recorder":
					deviceType = DeviceType.Recorder;
					break;

				case "MonitorBridgeYoko":
					deviceType = DeviceType.MonitorBridgeYoko;
					break;

				case "UnknownMgcpGateway":
					deviceType = DeviceType.UnknownMgcpGateway;
					break;

				case "Ipste2":
					deviceType = DeviceType.Ipste2;
					break;
			}

			return deviceType;
		}

		/// <summary>
		/// Converts from the name of an SCCP version to the corresponding enumerated value.
		/// </summary>
		/// <param name="protocol">Name of an SCCP version.</param>
		/// <returns>The corresponding enumerated value.</returns>
		private static ProtocolVersion ConvertVersion(string protocol)
		{
			ProtocolVersion version;

			switch (protocol)
			{
				case "Sp30":
					version = ProtocolVersion.Sp30;
					break;

				case "Bravo":
					version = ProtocolVersion.Bravo;
					break;

				case "Hawkbill":
					version = ProtocolVersion.Hawkbill;
					break;

				case "Seaview":
					version = ProtocolVersion.Seaview;
					break;

				default:
				case "Parche":
					version = ProtocolVersion.Parche;
					break;
			}

			return version;
		}

		/// <summary>
		/// Returns MAC address of this host if possible; otherwise, returns
		/// arbitrary, hard-coded MAC address.
		/// </summary>
		/// <returns>MAC address.</returns>
		internal static string GetMacAddress()
		{
			// Arbitrary MAC address if can't find one.
			string mac = "00:30:48:41:DF:69";

            try
            {
                //REFACTOR: Win32 specific
                foreach(ManagementObject mo in
                    new ManagementClass("Win32_NetworkAdapterConfiguration").GetInstances())
                {
                    if((bool)mo["IPEnabled"])
                    {
                        mac = (string)mo["MacAddress"];
                        break;
                    }
                }
            }
            catch {}

			return mac.Replace(":", "");
		}

		/// <summary>
		/// First IP address of this host or "any" host (0.0.0.0) if not
		/// available.
		/// </summary>
		private static string myIpAddress;

		/// <summary>
		/// First IP address of this host or "any" host (0.0.0.0) if not
		/// available.
		/// </summary>
		internal static string IpAddress { get { return myIpAddress; } }

		/// <summary>
		/// MAC address of this host or an arbitrary MAC address if not
		/// available.
		/// </summary>
		private static string myMacAddress;

		/// <summary>
		/// MAC address of this host or an arbitrary MAC address if not
		/// available.
		/// </summary>
		public static string MacAddress { get { return myMacAddress; } }
	}
}
