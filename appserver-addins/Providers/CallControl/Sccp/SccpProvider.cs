using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Core.IPC;
using Metreos.Core.ConfigData;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.SccpStack;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Messaging;
using Metreos.Messaging.MediaCaps;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Utilities;
using Metreos.Utilities.Collections;

using Msg=Metreos.Messaging;
using ConfigData=Metreos.Core.ConfigData;

namespace Metreos.CallControl.Sccp
{
	/// <summary>
	/// Represents the SCCP provider which mediates between the Telephony
	/// Manager and SCCP stack.
	/// </summary>
	[ProviderDecl("SCCP Call Control Provider")]
	public class SccpProvider : CallControlProviderBase
	{
        #region Construction/Initialization/Startup/Refresh/Shutdown

		/// <summary>
		/// Constructs an SccpProvider.
		/// </summary>
		/// <param name="configUtility">For accessing configuration
		/// data.</param>
		/// <param name="callIdFactory">Factory for generating unique call
		/// identifiers used between the Telephony Manager and all
		/// providers.</param>
        public SccpProvider(IConfigUtility configUtility, ICallIdFactory callIdFactory)
            : base(typeof(SccpProvider), Const.DisplayName, IConfig.ComponentType.SCCP_DevicePool, 
            configUtility, callIdFactory)
        {
			// Create the SCCP stack. Nothing happens, though, until we
			// register some SCCP devices.
            stack = new SccpStack.SccpStack(log);

			// Set to a bogus media address yet hopefully acceptable to
			// the CallManager.
			bitBucket = DetermineBitBucket();

			// Delegates used for device searches.
			registeredDevices = new RegisteredDevices();

			engagedDevices = new EngagedDevices(log);

			recentlyEngagedDevices =
				new BoundedCollection(Const.RecentlyEngagedDevicesMaximum);

			registrationThrottle = new ProcessObject();
			registrationThrottle.processObject +=
				new ProcessObjectDelegate(ProcessRegistrationMessage);

			// Construct wrapper for a collection of devices.
			devices = new Devices(this, configUtility, log, registeredDevices,
				engagedDevices, recentlyEngagedDevices, registrationThrottle);
		}

		/// <summary>
		/// Fixed-size collection of Device MAC addresses for the last N
		/// terminated calls.
		/// </summary>
		/// <remarks>
		/// Used to prevent Warnings from being generated when a provider Call
		/// is destroyed while a stack Call is still active and the provider
		/// receives events from the stack for them.
		/// </remarks>
		private readonly BoundedCollection recentlyEngagedDevices;

		/// <summary>
		/// Whether a Call object is to write Verbose diagnostics to the log.
		/// </summary>
		private bool logCallVerbose;

		/// <summary>
		/// Whether a Call object is to write Verbose diagnostics to the log.
		/// </summary>
		public bool IsLogCallVerbose { get { return logCallVerbose; } }

		/// <summary>
		/// Whether Music-On-Hold is enabled
		/// </summary>
		private bool mohOption;

		public bool IsMohEnabled { get { return mohOption; } }

		/// <summary>
		/// Internal registration messages are processed here in a separate
		/// thread.
		/// </summary>
		/// <remarks>
		/// This allows the ProcessObject to throttle the registration of
		/// devices so as to not overload the CallManager.
		/// </remarks>
		/// <param name="obj">Internal message to process.</param>
		private void ProcessRegistrationMessage(Object obj)
		{
			try
			{
				InternalRegistrationMessage message =
					obj as InternalRegistrationMessage;
				if (message != null)
				{
					message.Send();
				}
			}
			catch (ThreadAbortException)
			{
				// Do nothing.
			}
			catch (Exception e)
			{
				log.Write(TraceLevel.Error, "Prv: {0}", e);
			}
		}

		/// <summary>
		/// Determines what address/port to use as an RTP "bit bucket."
		/// </summary>
		/// <remarks>
		/// We sometimes signal this address as the temporary destination for
		/// an RTP stream. The protocol requires an address, yet we don't know
		/// where to route the RTP stream.
		/// </remarks>
		/// <returns></returns>
		private IPEndPoint DetermineBitBucket()
		{
			IPEndPoint address;

			IPAddress[] addresses = IpUtility.GetIPAddresses();
			if (addresses != null && addresses.Length > 0 &&
				addresses[0] != null)
			{
				// Set to a bogus media address yet hopefully acceptable to
				// the CallManager.
				address =
					new IPEndPoint(addresses[0], Const.BitBucketPortNumber);
			}
			else
			{
				log.Write(TraceLevel.Error, "Prv: no NICs");
				address = new IPEndPoint(IPAddress.Any, 0);
			}

			return address;
		}

		/// <summary>
		/// Initializes SccpProvider.
		/// </summary>
		/// <returns>Whether initialization is successful.</returns>
        protected override bool DeclareConfig(out ConfigEntry[] configItems, out Extension[] extensions)
        {
			string advanced = "Advanced: ";

            // There's so many of these, it's best to take a different approach
            ArrayList configArray = new ArrayList();

			// Provider parameters
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxBurst,
				Const.ConfigEntries.MaxBurst, Const.DefaultValues.MaxBurst,
				"Maximum registration messages per burst (" + Const.DefaultValues.MaxBurst.ToString() + ")",
				Const.Range.Min.MaxBurst, Const.Range.Max.MaxBurst, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.InterBurstDelayMs,
				Const.ConfigEntries.InterBurstDelayMs, Const.DefaultValues.InterBurstDelayMs,
				"Milliseconds between bursts (" + Const.DefaultValues.InterBurstDelayMs.ToString() + ")",
				Const.Range.Min.InterBurstDelayMs, Const.Range.Max.InterBurstDelayMs, true));

			// Stack parameters
			StringCollection versionFormatValues = new StringCollection();
			versionFormatValues.Add(Const.ConfigEntries.Versions.Sp30);
			versionFormatValues.Add(Const.ConfigEntries.Versions.Bravo);
			versionFormatValues.Add(Const.ConfigEntries.Versions.Hawkbill);
			versionFormatValues.Add(Const.ConfigEntries.Versions.Seaview);
			versionFormatValues.Add(Const.ConfigEntries.Versions.Parche);
			FormatType versionFormat =
				new FormatType(Const.ConfigValueFormats.Version,
				"SCCP version to report to CallManager", versionFormatValues);

			StringCollection deviceFormatValues = new StringCollection();
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.StationTelecasterMgr);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Station30spplus);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Station12spplus);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Station12sp);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Station12);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Station30vip);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.StationTelecaster);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.StationTelecasterBus);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.StationPolycom);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Station130spplus);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.StationPhoneApplication);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.AnalogAccess);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.DigitalAccessTitan1);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.DigitalAccessTitan2);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.DigitalAccessLennon);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.AnalogAccessElvis);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.ConferenceBridge);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.ConferenceBridgeYoko);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.H225);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.H323Phone);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.H323Trunk);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MusicOnHold);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Pilot);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.TapiPort);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.TapiRoutePoint);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.VoiceInbox);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.VoiceInboxAdmin);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.LineAnnunciator);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.SoftwareMtpDixieland);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.CiscoMediaServer);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.RouteList);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.LoadSimulator);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Ipste1);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MediaTerminationPoint);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MediaTerminationPointYoko);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MediaTerminationPointDixieland);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MediaTerminationPointSummit);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MgcpStation);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MgcpTrunk);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.RasProxy);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Trunk);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Annunciator);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MonitorBridge);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Recorder);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.MonitorBridgeYoko);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.UnknownMgcpGateway);
			deviceFormatValues.Add(Const.ConfigEntries.DeviceTypes.Ipste2);
			FormatType deviceFormat =
				new FormatType(Const.ConfigValueFormats.DeviceType,
				"SCCP device type to report to CallManager", deviceFormatValues);

			configArray.Add(new ConfigEntry(Const.ConfigEntries.CallManagerPort,
				Const.ConfigEntries.CallManagerPort, Const.DefaultValues.CallManagerPort,
				"Port on which CallManagers listen for registrations (" + Const.DefaultValues.CallManagerPort.ToString() + ")",
				Const.Range.Min.CallManagerPort, Const.Range.Max.CallManagerPort, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.AdvertiseLowBitRateCodecs,
				Const.ConfigEntries.AdvertiseLowBitRateCodecs, Const.DefaultValues.AdvertiseLowBitRateCodecs,
				"Whether devices should also be registered with G.729a support (" + Const.DefaultValues.AdvertiseLowBitRateCodecs + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MohOption,
				Const.ConfigEntries.MohOption, Const.DefaultValues.MohOption,
				"Whether Music-On-Hold is enabled (" + Const.DefaultValues.MohOption + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LogCallVerbose,
				Const.ConfigEntries.LogCallVerbose, Const.DefaultValues.LogCallVerbose,
				"Verbose logging for call (" + Const.DefaultValues.LogCallVerbose + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LogCallManagerVerbose,
				Const.ConfigEntries.LogCallManagerVerbose, Const.DefaultValues.LogCallManagerVerbose,
				"Verbose logging for CallManager (" + Const.DefaultValues.LogCallManagerVerbose + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LogConnectionVerbose,
				Const.ConfigEntries.LogConnectionVerbose, Const.DefaultValues.LogConnectionVerbose,
				"Verbose logging for connection (" + Const.DefaultValues.LogConnectionVerbose + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LogDiscoveryVerbose,
				Const.ConfigEntries.LogDiscoveryVerbose, Const.DefaultValues.LogDiscoveryVerbose,
				"Verbose logging for discovery (" + Const.DefaultValues.LogDiscoveryVerbose + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LogRegistrationVerbose,
				Const.ConfigEntries.LogRegistrationVerbose, Const.DefaultValues.LogRegistrationVerbose,
				"Verbose logging for registration (" + Const.DefaultValues.LogRegistrationVerbose + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LogSystemVerbose,
				Const.ConfigEntries.LogSystemVerbose, Const.DefaultValues.LogSystemVerbose,
				"Verbose logging for system (" + Const.DefaultValues.LogSystemVerbose + ")", IConfig.StandardFormat.Bool, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LingerSec,
				Const.ConfigEntries.LingerSec, Const.DefaultValues.LingerSec,
				advanced + "Number of seconds socket lingers after close (" + Const.DefaultValues.LingerSec.ToString() + ")",
				Const.Range.Min.LingerSec, Const.Range.Max.LingerSec, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.InitialTimerThreadpoolSize,
				Const.ConfigEntries.InitialTimerThreadpoolSize, Const.DefaultValues.InitialTimerThreadpoolSize,
				advanced + "Initial threads in the timer thread pool (" + Const.DefaultValues.InitialTimerThreadpoolSize.ToString() + ")",
				Const.Range.Min.InitialTimerThreadpoolSize, Const.Range.Max.InitialTimerThreadpoolSize, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxTimerThreadpoolSize,
				Const.ConfigEntries.MaxTimerThreadpoolSize, Const.DefaultValues.MaxTimerThreadpoolSize,
				advanced + "Maximum number of threads in the timer thread pool (" + Const.DefaultValues.MaxTimerThreadpoolSize.ToString() + ")",
				Const.Range.Min.MaxTimerThreadpoolSize, Const.Range.Max.MaxTimerThreadpoolSize, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.InitialSelectorActionThreadpoolSize,
				Const.ConfigEntries.InitialSelectorActionThreadpoolSize, Const.DefaultValues.InitialSelectorActionThreadpoolSize,
				advanced + "Initial threads in the selector-action thread pool (" + Const.DefaultValues.InitialSelectorActionThreadpoolSize.ToString() + ")",
				Const.Range.Min.InitialSelectorActionThreadpoolSize, Const.Range.Max.InitialSelectorActionThreadpoolSize, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxSelectorActionThreadpoolSize,
				Const.ConfigEntries.MaxSelectorActionThreadpoolSize, Const.DefaultValues.MaxSelectorActionThreadpoolSize,
				advanced + "Maximum number of threads in the selector-action thread pool (" + Const.DefaultValues.MaxSelectorActionThreadpoolSize.ToString() + ")",
				Const.Range.Min.MaxSelectorActionThreadpoolSize, Const.Range.Max.MaxSelectorActionThreadpoolSize, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MinWaitForKeepaliveAfterRegisterSec,
				Const.ConfigEntries.MinWaitForKeepaliveAfterRegisterSec, Const.DefaultValues.MinWaitForKeepaliveAfterRegisterSec,
				advanced + "Minimum wait before sending first Keepalive after registration in seconds (" + Const.DefaultValues.MinWaitForKeepaliveAfterRegisterSec.ToString() + ")",
				Const.Range.Min.MinWaitForKeepaliveAfterRegisterSec, Const.Range.Max.MinWaitForKeepaliveAfterRegisterSec, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxWaitForKeepaliveAfterRegisterSec,
				Const.ConfigEntries.MaxWaitForKeepaliveAfterRegisterSec, Const.DefaultValues.MaxWaitForKeepaliveAfterRegisterSec,
				advanced + "Maximum wait before sending first Keepalive after registration in seconds (" + Const.DefaultValues.MaxWaitForKeepaliveAfterRegisterSec.ToString() + ")",
				Const.Range.Min.MaxWaitForKeepaliveAfterRegisterSec, Const.Range.Max.MaxWaitForKeepaliveAfterRegisterSec, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.WaitingForRegisterAckKeepaliveMs,
				Const.ConfigEntries.WaitingForRegisterAckKeepaliveMs, Const.DefaultValues.WaitingForRegisterAckKeepaliveMs,
				advanced + "Waiting for RegisterAck Keepalive in milliseconds (" + Const.DefaultValues.WaitingForRegisterAckKeepaliveMs.ToString() + ")",
				Const.Range.Min.WaitingForRegisterAckKeepaliveMs, Const.Range.Max.WaitingForRegisterAckKeepaliveMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.RetryTokenRequestMs,
				Const.ConfigEntries.RetryTokenRequestMs, Const.DefaultValues.RetryTokenRequestMs,
				advanced + "Retry TokenRequest in milliseconds (" + Const.DefaultValues.RetryTokenRequestMs.ToString() + ")",
				Const.Range.Min.RetryTokenRequestMs, Const.Range.Max.RetryTokenRequestMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.StuckAlarmCount,
				Const.ConfigEntries.StuckAlarmCount, Const.DefaultValues.StuckAlarmCount,
				advanced + "Number of stuck alarms (" + Const.DefaultValues.StuckAlarmCount.ToString() + ")",
				Const.Range.Min.StuckAlarmCount, Const.Range.Max.StuckAlarmCount, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.CallManagerConnectRetries,
				Const.ConfigEntries.CallManagerConnectRetries, Const.DefaultValues.CallManagerConnectRetries,
				advanced + "Number of times to retry connecting to CallManager (" + Const.DefaultValues.CallManagerConnectRetries.ToString() + ")",
				Const.Range.Min.CallManagerConnectRetries, Const.Range.Max.CallManagerConnectRetries, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.SrstConnectRetries,
				Const.ConfigEntries.SrstConnectRetries, Const.DefaultValues.SrstConnectRetries,
				advanced + "Number of SRST connection retries (" + Const.DefaultValues.SrstConnectRetries.ToString() + ")",
				Const.Range.Min.SrstConnectRetries, Const.Range.Max.SrstConnectRetries, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.KeepaliveJitterPercent,
				Const.ConfigEntries.KeepaliveJitterPercent, Const.DefaultValues.KeepaliveJitterPercent,
				advanced + "Percent variance (+/-) applied to keepalive delay (" + Const.DefaultValues.KeepaliveJitterPercent.ToString() + ")",
				Const.Range.Min.KeepaliveJitterPercent, Const.Range.Max.KeepaliveJitterPercent, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.PadMs,
				Const.ConfigEntries.PadMs, Const.DefaultValues.PadMs,
				advanced + "Discovery pad in milliseconds (" + Const.DefaultValues.PadMs.ToString() + ")",
				Const.Range.Min.PadMs, Const.Range.Max.PadMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.WaitingForUnregisterMs,
				Const.ConfigEntries.WaitingForUnregisterMs, Const.DefaultValues.WaitingForUnregisterMs,
				advanced + "Waiting for Unregister in milliseconds (" + Const.DefaultValues.WaitingForUnregisterMs.ToString() + ")",
				Const.Range.Min.WaitingForUnregisterMs, Const.Range.Max.WaitingForUnregisterMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.NoCallManagersDefinedMs,
				Const.ConfigEntries.NoCallManagersDefinedMs, Const.DefaultValues.NoCallManagersDefinedMs,
				advanced + "No CallManagers defined in milliseconds (" + Const.DefaultValues.NoCallManagersDefinedMs.ToString() + ")",
				Const.Range.Min.NoCallManagersDefinedMs, Const.Range.Max.NoCallManagersDefinedMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.RetryPrimaryMs,
				Const.ConfigEntries.RetryPrimaryMs, Const.DefaultValues.RetryPrimaryMs,
				advanced + "Retry primary in milliseconds (" + Const.DefaultValues.RetryPrimaryMs.ToString() + ")",
				Const.Range.Min.RetryPrimaryMs, Const.Range.Max.RetryPrimaryMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxCallManagerListIterations,
				Const.ConfigEntries.MaxCallManagerListIterations, Const.DefaultValues.MaxCallManagerListIterations,
				advanced + "Retries by stack before giving up, after which provider registers (" + Const.DefaultValues.MaxCallManagerListIterations.ToString() + ")",
				Const.Range.Min.MaxCallManagerListIterations, Const.Range.Max.MaxCallManagerListIterations, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MinWaitForUnregisterSec,
				Const.ConfigEntries.MinWaitForUnregisterSec, Const.DefaultValues.MinWaitForUnregisterSec,
				advanced + "Minimum wait before sending Unregister in seconds (" + Const.DefaultValues.MinWaitForUnregisterSec.ToString() + ")",
				Const.Range.Min.MinWaitForUnregisterSec, Const.Range.Max.MinWaitForUnregisterSec, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxWaitForUnregisterSec,
				Const.ConfigEntries.MaxWaitForUnregisterSec, Const.DefaultValues.MaxWaitForUnregisterSec,
				advanced + "Maximum wait before sending Unregister in seconds (" + Const.DefaultValues.MaxWaitForUnregisterSec.ToString() + ")",
				Const.Range.Min.MaxWaitForUnregisterSec, Const.Range.Max.MaxWaitForUnregisterSec, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.RejectMs,
				Const.ConfigEntries.RejectMs, Const.DefaultValues.RejectMs,
				advanced + "Reject in milliseconds (" + Const.DefaultValues.RejectMs.ToString() + ")",
				Const.Range.Min.RejectMs, Const.Range.Max.RejectMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.AckRetriesBeforeLockout,
				Const.ConfigEntries.AckRetriesBeforeLockout, Const.DefaultValues.AckRetriesBeforeLockout,
				advanced + "Number of Ack retries before lockout (" + Const.DefaultValues.AckRetriesBeforeLockout.ToString() + ")",
				Const.Range.Min.AckRetriesBeforeLockout, Const.Range.Max.AckRetriesBeforeLockout, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.KeepaliveTimeoutsBeforeNoTokenRegister,
				Const.ConfigEntries.KeepaliveTimeoutsBeforeNoTokenRegister, Const.DefaultValues.KeepaliveTimeoutsBeforeNoTokenRegister,
				advanced + "Number of Keepalive timeouts before no TokenRegister (" + Const.DefaultValues.KeepaliveTimeoutsBeforeNoTokenRegister.ToString() + ")",
				Const.Range.Min.KeepaliveTimeoutsBeforeNoTokenRegister, Const.Range.Max.KeepaliveTimeoutsBeforeNoTokenRegister, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.UnregisterAckRetries,
				Const.ConfigEntries.UnregisterAckRetries, Const.DefaultValues.UnregisterAckRetries,
				advanced + "Number of UnregisterAck retries (" + Const.DefaultValues.UnregisterAckRetries.ToString() + ")",
				Const.Range.Min.UnregisterAckRetries, Const.Range.Max.UnregisterAckRetries, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxLines,
				Const.ConfigEntries.MaxLines, Const.DefaultValues.MaxLines,
				advanced + "Maximum number of lines per device (" + Const.DefaultValues.MaxLines.ToString() + ")",
				Const.Range.Min.MaxLines, Const.Range.Max.MaxLines, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxFeatures,
				Const.ConfigEntries.MaxFeatures, Const.DefaultValues.MaxFeatures,
				advanced + "Maximum number of features per device (" + Const.DefaultValues.MaxFeatures.ToString() + ")",
				Const.Range.Min.MaxFeatures, Const.Range.Max.MaxFeatures, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxServiceUrls,
				Const.ConfigEntries.MaxServiceUrls, Const.DefaultValues.MaxServiceUrls,
				advanced + "Maximum number of service URLs per device (" + Const.DefaultValues.MaxServiceUrls.ToString() + ")",
				Const.Range.Min.MaxServiceUrls, Const.Range.Max.MaxServiceUrls, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxSoftkeyDefinitions,
				Const.ConfigEntries.MaxSoftkeyDefinitions, Const.DefaultValues.MaxSoftkeyDefinitions,
				advanced + "Maximum number of Softkey definitions per device (" + Const.DefaultValues.MaxSoftkeyDefinitions.ToString() + ")",
				Const.Range.Min.MaxSoftkeyDefinitions, Const.Range.Max.MaxSoftkeyDefinitions, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxSoftkeySetDefinitions,
				Const.ConfigEntries.MaxSoftkeySetDefinitions, Const.DefaultValues.MaxSoftkeySetDefinitions,
				advanced + "Maximum number of SoftketSet definitions (" + Const.DefaultValues.MaxSoftkeySetDefinitions.ToString() + ")",
				Const.Range.Min.MaxSoftkeySetDefinitions, Const.Range.Max.MaxSoftkeySetDefinitions, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.MaxSpeeddials,
				Const.ConfigEntries.MaxSpeeddials, Const.DefaultValues.MaxSpeeddials,
				advanced + "Maximum number of speeddials per device (" + Const.DefaultValues.MaxSpeeddials.ToString() + ")",
				Const.Range.Min.MaxSpeeddials, Const.Range.Max.MaxSpeeddials, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.CallEndMs,
				Const.ConfigEntries.CallEndMs, Const.DefaultValues.CallEndMs,
				advanced + "Milliseconds to wait for all calls to go idle before checking for a new primary CallManager again (" + Const.DefaultValues.CallEndMs.ToString() + ")",
				Const.Range.Min.CallEndMs, Const.Range.Max.CallEndMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.CloseMs,
				Const.ConfigEntries.CloseMs, Const.DefaultValues.CloseMs,
				advanced + "Milliseconds to wait for connections to close before reset/restart (" + Const.DefaultValues.CloseMs.ToString() + ")",
				Const.Range.Min.CloseMs, Const.Range.Max.CloseMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.DevicePollMs,
				Const.ConfigEntries.DevicePollMs, Const.DefaultValues.DevicePollMs,
				advanced + "Device poll in milliseconds (" + Const.DefaultValues.DevicePollMs.ToString() + ")",
				Const.Range.Min.DevicePollMs, Const.Range.Max.DevicePollMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.DevicePollJitterPercent,
				Const.ConfigEntries.DevicePollJitterPercent, Const.DefaultValues.DevicePollJitterPercent,
				advanced + "Percent variance (+/-) applied to DevicePollMs (" + Const.DefaultValues.DevicePollJitterPercent.ToString() + ")",
				Const.Range.Min.DevicePollJitterPercent, Const.Range.Max.DevicePollJitterPercent, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.LockoutMs,
				Const.ConfigEntries.LockoutMs, Const.DefaultValues.LockoutMs,
				advanced + "Lockout timeout in milliseconds (" + Const.DefaultValues.LockoutMs.ToString() + ")",
				Const.Range.Min.LockoutMs, Const.Range.Max.LockoutMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.NakToSynRetryMs,
				Const.ConfigEntries.NakToSynRetryMs, Const.DefaultValues.NakToSynRetryMs,
				advanced + "Milliseconds after connect failure before retry (" + Const.DefaultValues.NakToSynRetryMs.ToString() + ")",
				Const.Range.Min.NakToSynRetryMs, Const.Range.Max.NakToSynRetryMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.ConnectMs,
				Const.ConfigEntries.ConnectMs, Const.DefaultValues.ConnectMs,
				advanced + "Milliseconds to wait after attempting to connect to a CallManager before making sure we have optimized CallManagers (" + Const.DefaultValues.ConnectMs.ToString() + ")",
				Const.Range.Min.ConnectMs, Const.Range.Max.ConnectMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.DefaultKeepaliveMs,
				Const.ConfigEntries.DefaultKeepaliveMs, Const.DefaultValues.DefaultKeepaliveMs,
				advanced + "Default Keepalive in milliseconds (" + Const.DefaultValues.DefaultKeepaliveMs.ToString() + ")",
				Const.Range.Min.DefaultKeepaliveMs, Const.Range.Max.DefaultKeepaliveMs, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.Version,
				Const.ConfigEntries.Version, Const.DefaultValues.Version,
				advanced + "SCCP version reported to CallManager (" + Const.DefaultValues.Version.ToString() + ")",
				versionFormat, true));
			configArray.Add(new ConfigEntry(Const.ConfigEntries.DeviceType,
				Const.ConfigEntries.DeviceType, Const.DefaultValues.DeviceType,
				advanced + "SCCP device type reported to CallManager (" + Const.DefaultValues.DeviceType.ToString() + ")",
				deviceFormat, true));

            // Copy dynamic config array to native array
            configItems = new ConfigEntry[configArray.Count];
            configArray.CopyTo(configItems);

			callManagerPort = Const.DefaultValues.CallManagerPort;
			SetCodecsByBitRate(Const.DefaultValues.AdvertiseLowBitRateCodecs);
			stack.LogCallVerbose = Const.DefaultValues.LogCallVerbose;
			stack.LogCallManagerVerbose = Const.DefaultValues.LogCallManagerVerbose;
			stack.LogConnectionVerbose = Const.DefaultValues.LogConnectionVerbose;
			stack.LogDiscoveryVerbose = Const.DefaultValues.LogDiscoveryVerbose;
			stack.LogRegistrationVerbose = Const.DefaultValues.LogRegistrationVerbose;
			stack.LogSystemVerbose = Const.DefaultValues.LogSystemVerbose;
			stack.LingerSec = Const.DefaultValues.LingerSec;
			stack.InitialTimerThreadpoolSize = Const.DefaultValues.InitialTimerThreadpoolSize;
			stack.MaxTimerThreadpoolSize = Const.DefaultValues.MaxTimerThreadpoolSize;
			stack.InitialSelectorActionThreadpoolSize = Const.DefaultValues.InitialSelectorActionThreadpoolSize;
			stack.MaxSelectorActionThreadpoolSize = Const.DefaultValues.MaxSelectorActionThreadpoolSize;
			stack.MinWaitForKeepaliveAfterRegisterSec = Const.DefaultValues.MinWaitForKeepaliveAfterRegisterSec;
			stack.MaxWaitForKeepaliveAfterRegisterSec = Const.DefaultValues.MaxWaitForKeepaliveAfterRegisterSec;
			stack.WaitingForRegisterAckKeepaliveMs = Const.DefaultValues.WaitingForRegisterAckKeepaliveMs;
			stack.RetryTokenRequestMs = Const.DefaultValues.RetryTokenRequestMs;
			stack.StuckAlarmCount = Const.DefaultValues.StuckAlarmCount;
			stack.CallManagerConnectRetries = Const.DefaultValues.CallManagerConnectRetries;
			stack.SrstConnectRetries = Const.DefaultValues.SrstConnectRetries;
			stack.KeepaliveJitterPercent = Const.DefaultValues.KeepaliveJitterPercent;
			stack.PadMs = Const.DefaultValues.PadMs;
			stack.WaitingForUnregisterMs = Const.DefaultValues.WaitingForUnregisterMs;
			stack.NoCallManagersDefinedMs = Const.DefaultValues.NoCallManagersDefinedMs;
			stack.RetryPrimaryMs = Const.DefaultValues.RetryPrimaryMs;
			stack.MaxCallManagerListIterations = Const.DefaultValues.MaxCallManagerListIterations;
			stack.MinWaitForUnregisterSec = Const.DefaultValues.MinWaitForUnregisterSec;
			stack.MaxWaitForUnregisterSec = Const.DefaultValues.MaxWaitForUnregisterSec;
			stack.RejectMs = Const.DefaultValues.RejectMs;
			stack.AckRetriesBeforeLockout = Const.DefaultValues.AckRetriesBeforeLockout;
			stack.KeepaliveTimeoutsBeforeNoTokenRegister = Const.DefaultValues.KeepaliveTimeoutsBeforeNoTokenRegister;
			stack.UnregisterAckRetries = Const.DefaultValues.UnregisterAckRetries;
			stack.MaxLines = Const.DefaultValues.MaxLines;
			stack.MaxFeatures = Const.DefaultValues.MaxFeatures;
			stack.MaxServiceUrls = Const.DefaultValues.MaxServiceUrls;
			stack.MaxSoftkeyDefinitions = Const.DefaultValues.MaxSoftkeyDefinitions;
			stack.MaxSoftkeySetDefinitions = Const.DefaultValues.MaxSoftkeySetDefinitions;
			stack.MaxSpeeddials = Const.DefaultValues.MaxSpeeddials;
			stack.CallEndMs = Const.DefaultValues.CallEndMs;
			stack.CloseMs = Const.DefaultValues.CloseMs;
			stack.DevicePollMs = Const.DefaultValues.DevicePollMs;
			stack.DevicePollJitterPercent = Const.DefaultValues.DevicePollJitterPercent;
			stack.LockoutMs = Const.DefaultValues.LockoutMs;
			stack.NakToSynRetryMs = Const.DefaultValues.NakToSynRetryMs;
			stack.ConnectMs = Const.DefaultValues.ConnectMs;
			stack.DefaultKeepaliveMs = Const.DefaultValues.DefaultKeepaliveMs;
			stack.Version = Const.DefaultValues.Version;
			stack.DeviceType_ = Const.DefaultValues.DeviceType;

            // No extensions
            extensions = null;

			return true;
        }

		/// <summary>
		/// Tell the SCCP stack which codec(s) the devices are to report to the
		/// CallManager as supporting. G.711 is always reported as being
		/// supported; the lowBitRate parameter indicates whether the
		/// low-bit-rate codec is also supported.
		/// </summary>
		/// <remarks>
		/// This method should only be called when there is no possibility that
		/// a device will register with a CallManager. This is because the
		/// Codecs collection being referenced here is not locked, thus
		/// exposing a thread-safety issue.
		/// </remarks>
		/// <param name="lowBitRate">Whether we support low-bit-rate codecs,
		/// i.e., just G.729a (we always support G.711 and Cisco doesn't
		/// support G.723.1).</param>
		private void SetCodecsByBitRate(bool lowBitRate)
		{
			stack.Codecs.Clear();

			if (lowBitRate)
			{
				// Indicate several variations of the low-bit-rate codec.
				stack.Codecs.Add(new CapabilitiesRes.MediaCapability(PayloadType.G729AnnexA, 20));
				stack.Codecs.Add(new CapabilitiesRes.MediaCapability(PayloadType.G729AnnexA, 30));
				stack.Codecs.Add(new CapabilitiesRes.MediaCapability(PayloadType.G729AnnexA, 40));
			}

			stack.Codecs.Add(new CapabilitiesRes.MediaCapability(PayloadType.G711Ulaw64k, 20));
		}

		/// <summary>
		/// Initializes SccpProvider further in the provider's own thread so
		/// that other providers can start up in parallel.
		/// </summary>
		/// <remarks>
		/// RefreshConfiguration() is executed within a separate thread so that
		/// we can return immediately. Otherwise, the ProviderManager might
		/// timeout before we finish registering devices.
		/// </remarks>
        protected override void OnStartup()
        {
			shuttingDown = false;

			if (!started)
			{
				registrationThrottle.Start();
				started = true;
			}

			RefreshConfiguration();
            
			base.RegisterNamespace();
		}

		/// <summary>
		/// Brings SccpProvider back to the state it was in before OnStartup()
		/// was called.
		/// </summary>
        protected override void OnShutdown()
		{
			shuttingDown = true;

			// Synchronize with ApplySccpDevicePoolConfiguration(). Having just
			// set shuttingDown to true should cause that method to return and
			// the thread within which it is executing to terminate.
			lock (devices.SyncRoot)
			{
				// Mark all SCCP devices as being unregistered, even though we
				// might not receive confirmations from the CallManager or even
				// though Unregister messages might not be sent. The CallManager
				// will soon mark them as unregistered because the TCP connection
				// will be terminated.
				configUtility.UpdateDeviceStatus(null, IConfig.DeviceType.Sccp,
					IConfig.Status.Enabled_Stopped);
				configUtility.SetDirectoryNumber(null,
					IConfig.DeviceType.Sccp, NotADirectoryNumber);

				ClearAndUnregisterDevices();

				stack.Stop();
			}
		}

		/// <summary>
		/// Reads possibly updated configuration values and installs them in
		/// the SccpProvider.
		/// </summary>
        protected override void RefreshConfiguration()
        {
			// Provider parameters
			int maxBurst = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxBurst));
			int interBurstDelayMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.InterBurstDelayMs));

			// Stack parameters
			int callManagerPort = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.CallManagerPort));
			bool advertiseLowBitRateCodecs  = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.AdvertiseLowBitRateCodecs));
			mohOption = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.MohOption));
			logCallVerbose = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.LogCallVerbose));
			bool logCallManagerVerbose = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.LogCallManagerVerbose));
			bool logConnectionVerbose = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.LogConnectionVerbose));
			bool logDiscoveryVerbose = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.LogDiscoveryVerbose));
			bool logRegistrationVerbose = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.LogRegistrationVerbose));
			bool logSystemVerbose = Convert.ToBoolean(GetConfigValue(Const.ConfigEntries.LogSystemVerbose));
			int lingerSec = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.LingerSec));
			int initialTimerThreadpoolSize = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.InitialTimerThreadpoolSize));
			int maxTimerThreadpoolSize = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxTimerThreadpoolSize));
			int initialSelectorActionThreadpoolSize = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.InitialSelectorActionThreadpoolSize));
			int maxSelectorActionThreadpoolSize = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxSelectorActionThreadpoolSize));
			int minWaitForKeepaliveAfterRegisterSec = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MinWaitForKeepaliveAfterRegisterSec));
			int maxWaitForKeepaliveAfterRegisterSec = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxWaitForKeepaliveAfterRegisterSec));
			int waitingForRegisterAckKeepaliveMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.WaitingForRegisterAckKeepaliveMs));
			int retryTokenRequestMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.RetryTokenRequestMs));
			int stuckAlarmCount = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.StuckAlarmCount));
			int callManagerConnectRetries = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.CallManagerConnectRetries));
			int srstConnectRetries = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.SrstConnectRetries));
			int keepaliveJitterPercent = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.KeepaliveJitterPercent));
			int padMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.PadMs));
			int waitingForUnregisterMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.WaitingForUnregisterMs));
			int noCallManagersDefinedMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.NoCallManagersDefinedMs));
			int retryPrimaryMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.RetryPrimaryMs));
			int maxCallManagerListIterations = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxCallManagerListIterations));
			int minWaitForUnregisterSec = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MinWaitForUnregisterSec));
			int maxWaitForUnregisterSec = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxWaitForUnregisterSec));
			int rejectMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.RejectMs));
			int ackRetriesBeforeLockout = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.AckRetriesBeforeLockout));
			int keepaliveTimeoutsBeforeNoTokenRegister = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.KeepaliveTimeoutsBeforeNoTokenRegister));
			int unregisterAckRetries = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.UnregisterAckRetries));
			int maxLines = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxLines));
			int maxFeatures = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxFeatures));
			int maxServiceUrls = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxServiceUrls));
			int maxSoftkeyDefinitions = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxSoftkeyDefinitions));
			int maxSoftkeySetDefinitions = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxSoftkeySetDefinitions));
			int maxSpeeddials = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.MaxSpeeddials));
			int callEndMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.CallEndMs));
			int closeMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.CloseMs));
			int devicePollMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.DevicePollMs));
			int devicePollJitterPercent = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.DevicePollJitterPercent));
			int lockoutMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.LockoutMs));
			int nakToSynRetryMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.NakToSynRetryMs));
			int connectMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.ConnectMs));
			int defaultKeepaliveMs = Convert.ToInt32(GetConfigValue(Const.ConfigEntries.DefaultKeepaliveMs));
			string version = Convert.ToString(base.GetConfigValue(Const.ConfigEntries.Version));
			string deviceType = Convert.ToString(base.GetConfigValue(Const.ConfigEntries.DeviceType));

			#region Sanity checks
			// Provider parameters
			if (maxBurst < Const.Range.Min.MaxBurst ||
				maxBurst > Const.Range.Max.MaxBurst)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxBurst = {0}. Ignoring",
					maxBurst);
			}
			else
			{
				registrationThrottle.MaxBurst = maxBurst;
			}

			if (interBurstDelayMs < Const.Range.Min.InterBurstDelayMs ||
				interBurstDelayMs > Const.Range.Max.InterBurstDelayMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: InterBurstDelayMs = {0}. Ignoring",
					interBurstDelayMs);
			}
			else
			{
				registrationThrottle.InterBurstDelayMs = interBurstDelayMs;
			}

			// Stack parameters
			if (callManagerPort < Const.Range.Min.CallManagerPort ||
				callManagerPort > Const.Range.Max.CallManagerPort)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: CallManagerPort = {0}. Ignoring",
					callManagerPort);
			}
			else
			{
				this.callManagerPort = callManagerPort;
			}

			SetCodecsByBitRate(advertiseLowBitRateCodecs);

			stack.LogCallVerbose = logCallVerbose;

			stack.LogCallManagerVerbose = logCallManagerVerbose;

			stack.LogConnectionVerbose = logConnectionVerbose;

			stack.LogDiscoveryVerbose = logDiscoveryVerbose;

			stack.LogRegistrationVerbose = logRegistrationVerbose;

			stack.LogSystemVerbose = logSystemVerbose;

			if (lingerSec < Const.Range.Min.LingerSec ||
				lingerSec > Const.Range.Max.LingerSec)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: LingerSec = {0}. Ignoring",
					lingerSec);
			}
			else
			{
				stack.LingerSec = lingerSec;
			}

			if (initialTimerThreadpoolSize < Const.Range.Min.InitialTimerThreadpoolSize ||
				initialTimerThreadpoolSize > Const.Range.Max.InitialTimerThreadpoolSize)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: InitialTimerThreadpoolSize = {0}. Ignoring",
					initialTimerThreadpoolSize);
			}
			else
			{
				stack.InitialTimerThreadpoolSize = initialTimerThreadpoolSize;
			}

			if (maxTimerThreadpoolSize < Const.Range.Min.MaxTimerThreadpoolSize ||
				maxTimerThreadpoolSize > Const.Range.Max.MaxTimerThreadpoolSize)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxTimerThreadpoolSize = {0}. Ignoring",
					maxTimerThreadpoolSize);
			}
			else
			{
				stack.MaxTimerThreadpoolSize = maxTimerThreadpoolSize;
			}

			if (initialSelectorActionThreadpoolSize < Const.Range.Min.InitialSelectorActionThreadpoolSize ||
				initialSelectorActionThreadpoolSize > Const.Range.Max.InitialSelectorActionThreadpoolSize)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: InitialSelectorActionThreadpoolSize = {0}. Ignoring",
					initialSelectorActionThreadpoolSize);
			}
			else
			{
				stack.InitialSelectorActionThreadpoolSize = initialSelectorActionThreadpoolSize;
			}

			if (maxSelectorActionThreadpoolSize < Const.Range.Min.MaxSelectorActionThreadpoolSize ||
				maxSelectorActionThreadpoolSize > Const.Range.Max.MaxSelectorActionThreadpoolSize)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxSelectorActionThreadpoolSize = {0}. Ignoring",
					maxSelectorActionThreadpoolSize);
			}
			else
			{
				stack.MaxSelectorActionThreadpoolSize = maxSelectorActionThreadpoolSize;
			}

			if (minWaitForKeepaliveAfterRegisterSec < Const.Range.Min.MinWaitForKeepaliveAfterRegisterSec ||
				minWaitForKeepaliveAfterRegisterSec > Const.Range.Max.MinWaitForKeepaliveAfterRegisterSec)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MinWaitForKeepaliveAfterRegisterSec = {0}. Ignoring",
					minWaitForKeepaliveAfterRegisterSec);
			}
			else
			{

				if (maxWaitForKeepaliveAfterRegisterSec < Const.Range.Min.MaxWaitForKeepaliveAfterRegisterSec ||
					maxWaitForKeepaliveAfterRegisterSec > Const.Range.Max.MaxWaitForKeepaliveAfterRegisterSec)
				{
					log.Write(TraceLevel.Warning,
						"Prv: invalid Config: MaxWaitForKeepaliveAfterRegisterSec = {0}. Ignoring",
						maxWaitForKeepaliveAfterRegisterSec);
				}
				else
				{
					if (maxWaitForKeepaliveAfterRegisterSec < minWaitForKeepaliveAfterRegisterSec)
					{
						log.Write(TraceLevel.Warning,
							"Prv: invalid Config: MaxWaitForKeepaliveAfterRegisterSec ({0}) < MinWaitForKeepaliveAfterRegisterSec ({1}). Ignoring both",
							maxWaitForKeepaliveAfterRegisterSec, minWaitForKeepaliveAfterRegisterSec);
					}
					else
					{
						stack.MinWaitForKeepaliveAfterRegisterSec = minWaitForKeepaliveAfterRegisterSec;
						stack.MaxWaitForKeepaliveAfterRegisterSec = maxWaitForKeepaliveAfterRegisterSec;
					}
				}
			}

			if (waitingForRegisterAckKeepaliveMs < Const.Range.Min.WaitingForRegisterAckKeepaliveMs ||
				waitingForRegisterAckKeepaliveMs > Const.Range.Max.WaitingForRegisterAckKeepaliveMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: WaitingForRegisterAckKeepaliveMs = {0}. Ignoring",
					waitingForRegisterAckKeepaliveMs);
			}
			else
			{
				stack.WaitingForRegisterAckKeepaliveMs = waitingForRegisterAckKeepaliveMs;
			}

			if (retryTokenRequestMs < Const.Range.Min.RetryTokenRequestMs ||
				retryTokenRequestMs > Const.Range.Max.RetryTokenRequestMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: RetryTokenRequestMs = {0}. Ignoring",
					retryTokenRequestMs);
			}
			else
			{
				stack.RetryTokenRequestMs = retryTokenRequestMs;
			}

			if (stuckAlarmCount < Const.Range.Min.StuckAlarmCount ||
				stuckAlarmCount > Const.Range.Max.StuckAlarmCount)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: StuckAlarmCount = {0}. Ignoring",
					stuckAlarmCount);
			}
			else
			{
				stack.StuckAlarmCount = stuckAlarmCount;
			}

			if (callManagerConnectRetries < Const.Range.Min.CallManagerConnectRetries ||
				callManagerConnectRetries > Const.Range.Max.CallManagerConnectRetries)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: CallManagerConnectRetries = {0}. Ignoring",
					callManagerConnectRetries);
			}
			else
			{
				stack.CallManagerConnectRetries = callManagerConnectRetries;
			}

			if (srstConnectRetries < Const.Range.Min.SrstConnectRetries ||
				srstConnectRetries > Const.Range.Max.SrstConnectRetries)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: SrstConnectRetries = {0}. Ignoring",
					srstConnectRetries);
			}
			else
			{
				stack.SrstConnectRetries = srstConnectRetries;
			}

			if (keepaliveJitterPercent < Const.Range.Min.KeepaliveJitterPercent ||
				keepaliveJitterPercent > Const.Range.Max.KeepaliveJitterPercent)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: KeepaliveJitterPercent = {0}. Ignoring",
					keepaliveJitterPercent);
			}
			else
			{
				stack.KeepaliveJitterPercent = keepaliveJitterPercent;
			}

			if (padMs < Const.Range.Min.PadMs ||
				padMs > Const.Range.Max.PadMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: PadMs = {0}. Ignoring",
					padMs);
			}
			else
			{
				stack.PadMs = padMs;
			}

			if (waitingForUnregisterMs < Const.Range.Min.WaitingForUnregisterMs ||
				waitingForUnregisterMs > Const.Range.Max.WaitingForUnregisterMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: WaitingForUnregisterMs = {0}. Ignoring",
					waitingForUnregisterMs);
			}
			else
			{
				stack.WaitingForUnregisterMs = waitingForUnregisterMs;
			}

			if (noCallManagersDefinedMs < Const.Range.Min.NoCallManagersDefinedMs ||
				noCallManagersDefinedMs > Const.Range.Max.NoCallManagersDefinedMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: NoCallManagersDefinedMs = {0}. Ignoring",
					noCallManagersDefinedMs);
			}
			else
			{
				stack.NoCallManagersDefinedMs = noCallManagersDefinedMs;
			}

			if (retryPrimaryMs < Const.Range.Min.RetryPrimaryMs ||
				retryPrimaryMs > Const.Range.Max.RetryPrimaryMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: RetryPrimaryMs = {0}. Ignoring",
					retryPrimaryMs);
			}
			else
			{
				stack.RetryPrimaryMs = retryPrimaryMs;
			}

			if (maxCallManagerListIterations < Const.Range.Min.MaxCallManagerListIterations ||
				maxCallManagerListIterations > Const.Range.Max.MaxCallManagerListIterations)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxCallManagerListIterations = {0}. Ignoring",
					maxCallManagerListIterations);
			}
			else
			{
				stack.MaxCallManagerListIterations = maxCallManagerListIterations;
			}

			if (minWaitForUnregisterSec < Const.Range.Min.MinWaitForUnregisterSec ||
				minWaitForUnregisterSec > Const.Range.Max.MinWaitForUnregisterSec)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MinWaitForUnregisterSec = {0}. Ignoring",
					minWaitForUnregisterSec);
			}
			else
			{

				if (maxWaitForUnregisterSec < Const.Range.Min.MaxWaitForUnregisterSec ||
					maxWaitForUnregisterSec > Const.Range.Max.MaxWaitForUnregisterSec)
				{
					log.Write(TraceLevel.Warning,
						"Prv: invalid Config: MaxWaitForUnregisterSec = {0}. Ignoring",
						maxWaitForUnregisterSec);
				}
				else
				{
					if (maxWaitForUnregisterSec < minWaitForUnregisterSec)
					{
						log.Write(TraceLevel.Warning,
							"Prv: invalid Config: MaxWaitForUnregisterSec ({0}) < MinWaitForUnregisterSec ({1}). Ignoring both",
							maxWaitForUnregisterSec, minWaitForUnregisterSec);
					}
					else
					{
						stack.MinWaitForUnregisterSec = minWaitForUnregisterSec;
						stack.MaxWaitForUnregisterSec = maxWaitForUnregisterSec;
					}
				}
			}

			if (rejectMs < Const.Range.Min.RejectMs ||
				rejectMs > Const.Range.Max.RejectMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: RejectMs = {0}. Ignoring",
					rejectMs);
			}
			else
			{
				stack.RejectMs = rejectMs;
			}

			if (ackRetriesBeforeLockout < Const.Range.Min.AckRetriesBeforeLockout ||
				ackRetriesBeforeLockout > Const.Range.Max.AckRetriesBeforeLockout)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: AckRetriesBeforeLockout = {0}. Ignoring",
					ackRetriesBeforeLockout);
			}
			else
			{
				stack.AckRetriesBeforeLockout = ackRetriesBeforeLockout;
			}

			if (keepaliveTimeoutsBeforeNoTokenRegister < Const.Range.Min.KeepaliveTimeoutsBeforeNoTokenRegister ||
				keepaliveTimeoutsBeforeNoTokenRegister > Const.Range.Max.KeepaliveTimeoutsBeforeNoTokenRegister)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: KeepaliveTimeoutsBeforeNoTokenRegister = {0}. Ignoring",
					keepaliveTimeoutsBeforeNoTokenRegister);
			}
			else
			{
				stack.KeepaliveTimeoutsBeforeNoTokenRegister = keepaliveTimeoutsBeforeNoTokenRegister;
			}

			if (unregisterAckRetries < Const.Range.Min.UnregisterAckRetries ||
				unregisterAckRetries > Const.Range.Max.UnregisterAckRetries)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: UnregisterAckRetries = {0}. Ignoring",
					unregisterAckRetries);
			}
			else
			{
				stack.UnregisterAckRetries = unregisterAckRetries;
			}

			if (maxLines < Const.Range.Min.MaxLines ||
				maxLines > Const.Range.Max.MaxLines)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxLines = {0}. Ignoring",
					maxLines);
			}
			else
			{
				stack.MaxLines = maxLines;
			}

			if (maxFeatures < Const.Range.Min.MaxFeatures ||
				maxFeatures > Const.Range.Max.MaxFeatures)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxFeatures = {0}. Ignoring",
					maxFeatures);
			}
			else
			{
				stack.MaxFeatures = maxFeatures;
			}

			if (maxServiceUrls < Const.Range.Min.MaxServiceUrls ||
				maxServiceUrls > Const.Range.Max.MaxServiceUrls)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxServiceUrls = {0}. Ignoring",
					maxServiceUrls);
			}
			else
			{
				stack.MaxServiceUrls = maxServiceUrls;
			}

			if (maxSoftkeyDefinitions < Const.Range.Min.MaxSoftkeyDefinitions ||
				maxSoftkeyDefinitions > Const.Range.Max.MaxSoftkeyDefinitions)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxSoftkeyDefinitions = {0}. Ignoring",
					maxSoftkeyDefinitions);
			}
			else
			{
				stack.MaxSoftkeyDefinitions = maxSoftkeyDefinitions;
			}

			if (maxSoftkeySetDefinitions < Const.Range.Min.MaxSoftkeySetDefinitions ||
				maxSoftkeySetDefinitions > Const.Range.Max.MaxSoftkeySetDefinitions)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxSoftkeySetDefinitions = {0}. Ignoring",
					maxSoftkeySetDefinitions);
			}
			else
			{
				stack.MaxSoftkeySetDefinitions = maxSoftkeySetDefinitions;
			}

			if (maxSpeeddials < Const.Range.Min.MaxSpeeddials ||
				maxSpeeddials > Const.Range.Max.MaxSpeeddials)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: MaxSpeeddials = {0}. Ignoring",
					maxSpeeddials);
			}
			else
			{
				stack.MaxSpeeddials = maxSpeeddials;
			}

			if (callEndMs < Const.Range.Min.CallEndMs ||
				callEndMs > Const.Range.Max.CallEndMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: CallEndMs = {0}. Ignoring",
					callEndMs);
			}
			else
			{
				stack.CallEndMs = callEndMs;
			}

			if (closeMs < Const.Range.Min.CloseMs ||
				closeMs > Const.Range.Max.CloseMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: CloseMs = {0}. Ignoring",
					closeMs);
			}
			else
			{
				stack.CloseMs = closeMs;
			}

			if (devicePollMs < Const.Range.Min.DevicePollMs ||
				devicePollMs > Const.Range.Max.DevicePollMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: DevicePollMs = {0}. Ignoring",
					devicePollMs);
			}
			else
			{
				stack.DevicePollMs = devicePollMs;
			}

			if (devicePollJitterPercent < Const.Range.Min.DevicePollJitterPercent ||
				devicePollJitterPercent > Const.Range.Max.DevicePollJitterPercent)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: DevicePollJitterPercent = {0}. Ignoring",
					devicePollJitterPercent);
			}
			else
			{
				stack.DevicePollJitterPercent = devicePollJitterPercent;
			}

			if (lockoutMs < Const.Range.Min.LockoutMs ||
				lockoutMs > Const.Range.Max.LockoutMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: LockoutMs = {0}. Ignoring",
					lockoutMs);
			}
			else
			{
				stack.LockoutMs = lockoutMs;
			}

			if (nakToSynRetryMs < Const.Range.Min.NakToSynRetryMs ||
				nakToSynRetryMs > Const.Range.Max.NakToSynRetryMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: NakToSynRetryMs = {0}. Ignoring",
					nakToSynRetryMs);
			}
			else
			{
				stack.NakToSynRetryMs = nakToSynRetryMs;
			}

			if (connectMs < Const.Range.Min.ConnectMs ||
				connectMs > Const.Range.Max.ConnectMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: ConnectMs = {0}. Ignoring",
					connectMs);
			}
			else
			{
				stack.ConnectMs = connectMs;
			}

			if (defaultKeepaliveMs < Const.Range.Min.DefaultKeepaliveMs ||
				defaultKeepaliveMs > Const.Range.Max.DefaultKeepaliveMs)
			{
				log.Write(TraceLevel.Warning,
					"Prv: invalid Config: DefaultKeepaliveMs = {0}. Ignoring",
					defaultKeepaliveMs);
			}
			else
			{
				stack.DefaultKeepaliveMs = defaultKeepaliveMs;
			}

			if (version == null || version == string.Empty)
			{
				log.Write(TraceLevel.Error, "Prv: no value entered for SCCP version. Ignoring");
			}
			else
			{
				switch (version)
				{
					case Const.ConfigEntries.Versions.Sp30:
					case Const.ConfigEntries.Versions.Bravo:
					case Const.ConfigEntries.Versions.Hawkbill:
					case Const.ConfigEntries.Versions.Seaview:
					case Const.ConfigEntries.Versions.Parche:
						stack.Version = version;
						break;

					default:
						log.Write(TraceLevel.Warning, "Prv: invalid Config: SCCP Version {0} invalid. Ignoring",
							version);
						break;
				}
			}

			if (deviceType == null || deviceType == string.Empty)
			{
				log.Write(TraceLevel.Error, "Prv: no value entered for SCCP device type. Ignoring");
			}
			else
			{
				switch (deviceType)
				{
					case Const.ConfigEntries.DeviceTypes.StationTelecasterMgr:
					case Const.ConfigEntries.DeviceTypes.Station30spplus:
					case Const.ConfigEntries.DeviceTypes.Station12spplus:
					case Const.ConfigEntries.DeviceTypes.Station12sp:
					case Const.ConfigEntries.DeviceTypes.Station12:
					case Const.ConfigEntries.DeviceTypes.Station30vip:
					case Const.ConfigEntries.DeviceTypes.StationTelecaster:
					case Const.ConfigEntries.DeviceTypes.StationTelecasterBus:
					case Const.ConfigEntries.DeviceTypes.StationPolycom:
					case Const.ConfigEntries.DeviceTypes.Station130spplus:
					case Const.ConfigEntries.DeviceTypes.StationPhoneApplication:
					case Const.ConfigEntries.DeviceTypes.AnalogAccess:
					case Const.ConfigEntries.DeviceTypes.DigitalAccessTitan1:
					case Const.ConfigEntries.DeviceTypes.DigitalAccessTitan2:
					case Const.ConfigEntries.DeviceTypes.DigitalAccessLennon:
					case Const.ConfigEntries.DeviceTypes.AnalogAccessElvis:
					case Const.ConfigEntries.DeviceTypes.ConferenceBridge:
					case Const.ConfigEntries.DeviceTypes.ConferenceBridgeYoko:
					case Const.ConfigEntries.DeviceTypes.H225:
					case Const.ConfigEntries.DeviceTypes.H323Phone:
					case Const.ConfigEntries.DeviceTypes.H323Trunk:
					case Const.ConfigEntries.DeviceTypes.MusicOnHold:
					case Const.ConfigEntries.DeviceTypes.Pilot:
					case Const.ConfigEntries.DeviceTypes.TapiPort:
					case Const.ConfigEntries.DeviceTypes.TapiRoutePoint:
					case Const.ConfigEntries.DeviceTypes.VoiceInbox:
					case Const.ConfigEntries.DeviceTypes.VoiceInboxAdmin:
					case Const.ConfigEntries.DeviceTypes.LineAnnunciator:
					case Const.ConfigEntries.DeviceTypes.SoftwareMtpDixieland:
					case Const.ConfigEntries.DeviceTypes.CiscoMediaServer:
					case Const.ConfigEntries.DeviceTypes.RouteList:
					case Const.ConfigEntries.DeviceTypes.LoadSimulator:
					case Const.ConfigEntries.DeviceTypes.Ipste1:
					case Const.ConfigEntries.DeviceTypes.MediaTerminationPoint:
					case Const.ConfigEntries.DeviceTypes.MediaTerminationPointYoko:
					case Const.ConfigEntries.DeviceTypes.MediaTerminationPointDixieland:
					case Const.ConfigEntries.DeviceTypes.MediaTerminationPointSummit:
					case Const.ConfigEntries.DeviceTypes.MgcpStation:
					case Const.ConfigEntries.DeviceTypes.MgcpTrunk:
					case Const.ConfigEntries.DeviceTypes.RasProxy:
					case Const.ConfigEntries.DeviceTypes.Trunk:
					case Const.ConfigEntries.DeviceTypes.Annunciator:
					case Const.ConfigEntries.DeviceTypes.MonitorBridge:
					case Const.ConfigEntries.DeviceTypes.Recorder:
					case Const.ConfigEntries.DeviceTypes.MonitorBridgeYoko:
					case Const.ConfigEntries.DeviceTypes.UnknownMgcpGateway:
					case Const.ConfigEntries.DeviceTypes.Ipste2:
						stack.DeviceType_ = deviceType;
						break;

					default:
						log.Write(TraceLevel.Warning, "Prv: invalid Config: SCCP Device Type {0} invalid. Ignoring",
							deviceType);
						break;
				}
			}
			#endregion

			if (started)
			{
				ApplySccpDevicePoolConfiguration();
			}
        }

		/// <summary>
		/// Applies the currently configured SCCP device pool--makes sure only
		/// the specified devices are registered.
		/// </summary>
		private void ApplySccpDevicePoolConfiguration()
		{
			// Synchronize with OnShutdown().
			lock (devices.SyncRoot)
			{
				// Register devices
				ConfigData.ComponentInfo[] callManagers = 
					configUtility.GetComponents(
					IConfig.ComponentType.SCCP_DevicePool);

				// If no CallManagers specified at all, unregister all devices.
				if (callManagers == null)
				{
					ClearAndUnregisterDevices();
				}
				else
				{
					// Mark all devices as not having been checked yet for whether
					// they need to be registered.
					devices.ClearConfirmationFlags();

					// Check each CallManager's SCCP device pool (if it is
					// configured for one).
					foreach (ConfigData.ComponentInfo cmInfo in callManagers)
					{
						ConfigData.DeviceInfo[] sccpDevices =
							configUtility.GetSccpDevices(cmInfo);
						if (sccpDevices != null)
						{
							// Now check each device in a pool. If it is not
							// registered already, register it.
							foreach (ConfigData.DeviceInfo dInfo in sccpDevices)
							{
								AssureDeviceIsRegistered(dInfo);
								if (shuttingDown)
								{
									break;
								}
							}
						}
						if (shuttingDown)
						{
							break;
						}
					}

					// Unregister devices that were registered but are no longer
					// present in an SCCP device pool.
					// (Expedite only if we happen to be shutting down.)
					devices.ClearUnconfirmedDevices(shuttingDown);
				}
			}
		}

		/// <summary>
		/// Removes every Device from the registeredDevices and devices
		/// collections and unregisters them.
		/// </summary>
		private void ClearAndUnregisterDevices()
		{
            lock(devices.SyncRoot)
            {
                foreach(DictionaryEntry de in devices)
                {
                    Device device = de.Value as Device;
                    if(device != null && device.InUse)
                    {
                        base.SendHangup(device.Call.CallId, ICallControl.EndReason.Normal);
                    }
                }

                devices.Clear(shuttingDown);
                registeredDevices.Clear();
				engagedDevices.Clear();
				recentlyEngagedDevices.Clear();
            }
		}

		/// <summary>
		/// Makes sure the specified device is registered.
		/// </summary>
		/// <param name="dInfo">Device to check for registration.</param>
		private void AssureDeviceIsRegistered(ConfigData.DeviceInfo dInfo)
		{
			// (Don't know how or why a device would not have a name. Should
			// we log an Error for this?)
			if (dInfo.Name != null)
			{
				if (devices.ContainsName(dInfo.Name))
				{
					// Already registered so no need to register it again.
					// Just mark it has having been confirmed registered.
					devices.Confirm(dInfo.Name);
				}
				else
				{
					// Create an SCCP device for this device to communicate
					// with its CallManagers, and initiate the registration
					// process.
					SccpStack.Device device = CreateSession();
					if (device == null)
					{
						log.Write(TraceLevel.Error,
							"Prv: failed to create SCCP device for device: {0}",
							dInfo.Name);
					}
					else
					{
						devices.CreateDevice(dInfo.Name, device);
						devices.Register(dInfo.Name, dInfo.ServerAddrs,
							dInfo.FailoverServerAddrs == null ? new IPAddress[0] : dInfo.FailoverServerAddrs,
							callManagerPort);
                                            
						log.Write(TraceLevel.Info,
							"Prv: registering: {0}@{1}", dInfo.Name,
							dInfo.ServerAddrs[0].ToString());
					}
				}
			}
		}

        #endregion

		#region Constants

		/// <summary>
		/// Constants referenced within this class.
		/// </summary>
		private abstract class Const
		{
			public const string DisplayName = "SCCP Provider";

			/// <summary>
			/// The most Device MAC addresses to hold in the
			/// recentlyEngagedDevices collection.
			/// </summary>
			public const int RecentlyEngagedDevicesMaximum = 100;

			/// <summary>
			/// Ostensibly unused port where we can direct incoming RTP traffic
			/// about which we don't care.
			/// </summary>
			/// <remarks>
			/// IANA indicates that this is a "Reserved" port, so let's hope
			/// it's safe (http://www.iana.org/assignments/port-numbers).
			/// </remarks>
			public const int BitBucketPortNumber = 254;

			public abstract class ConfigEntries
			{
				// Provider parameters
				public const string MaxBurst = "MaxBurst";
				public const string InterBurstDelayMs = "InterBurstDelayMs";

				// Stack parameters
				public const string CallManagerPort = "CallManagerPort";
				public const string AdvertiseLowBitRateCodecs = "AdvertiseLowBitRateCodecs";
				public const string LingerSec = "LingerSec";
				public const string InitialTimerThreadpoolSize = "InitialTimerThreadpoolSize";
				public const string MaxTimerThreadpoolSize = "MaxTimerThreadpoolSize";
				public const string InitialSelectorActionThreadpoolSize = "InitialSelectorActionThreadpoolSize";
				public const string MaxSelectorActionThreadpoolSize = "MaxSelectorActionThreadpoolSize";
				public const string MinWaitForKeepaliveAfterRegisterSec = "MinWaitForKeepaliveAfterRegisterSec";
				public const string MaxWaitForKeepaliveAfterRegisterSec = "MaxWaitForKeepaliveAfterRegisterSec";
				public const string WaitingForRegisterAckKeepaliveMs = "WaitingForRegisterAckKeepaliveMs";
				public const string RetryTokenRequestMs = "RetryTokenRequestMs";
				public const string StuckAlarmCount = "StuckAlarmCount";
				public const string CallManagerConnectRetries = "CallManagerConnectRetries";
				public const string SrstConnectRetries = "SrstConnectRetries";
				public const string KeepaliveJitterPercent = "KeepaliveJitterPercent";
				public const string PadMs = "PadMs";
				public const string WaitingForUnregisterMs = "WaitingForUnregisterMs";
				public const string NoCallManagersDefinedMs = "NoCallManagersDefinedMs";
				public const string RetryPrimaryMs = "RetryPrimaryMs";
				public const string MaxCallManagerListIterations = "MaxCallManagerListIterations";
				public const string MinWaitForUnregisterSec = "MinWaitForUnregisterSec";
				public const string MaxWaitForUnregisterSec = "MaxWaitForUnregisterSec";
				public const string RejectMs = "RejectMs";
				public const string AckRetriesBeforeLockout = "AckRetriesBeforeLockout";
				public const string KeepaliveTimeoutsBeforeNoTokenRegister = "KeepaliveTimeoutsBeforeNoTokenRegister";
				public const string UnregisterAckRetries = "UnregisterAckRetries";
				public const string DevicePollJitterPercent = "DevicePollJitterPercent";
				public const string MaxLines = "MaxLines";
				public const string MaxFeatures = "MaxFeatures";
				public const string MaxServiceUrls = "MaxServiceUrls";
				public const string MaxSoftkeyDefinitions = "MaxSoftkeyDefinitions";
				public const string MaxSoftkeySetDefinitions = "MaxSoftkeySetDefinitions";
				public const string MaxSpeeddials = "MaxSpeeddials";
				public const string CallEndMs = "CallEndMs";
				public const string CloseMs = "CloseMs";
				public const string DevicePollMs = "DevicePollMs";
				public const string LockoutMs = "LockoutMs";
				public const string NakToSynRetryMs = "NakToSynRetryMs";
				public const string ConnectMs = "ConnectMs";
				public const string DefaultKeepaliveMs = "DefaultKeepaliveMs";
				public const string Version = "Version";
				public const string DeviceType = "DeviceType";
				public const string LogCallVerbose = "LogCallVerbose";
				public const string LogCallManagerVerbose = "LogCallManagerVerbose";
				public const string LogConnectionVerbose = "LogConnectionVerbose";
				public const string LogDiscoveryVerbose = "LogDiscoveryVerbose";
				public const string LogRegistrationVerbose = "LogRegistrationVerbose";
				public const string LogSystemVerbose = "LogSystemVerbose";

				public const string MohOption = "MusicOnHoldOption";

				public abstract class Versions
				{
					public const string Sp30 = "Sp30";
					public const string Bravo = "Bravo";
					public const string Hawkbill = "Hawkbill";
					public const string Seaview = "Seaview";
					public const string Parche = "Parche";
				}

				public abstract class DeviceTypes
				{
					public const string StationTelecasterMgr = "StationTelecasterMgr";
					public const string Station30spplus = "Station30spplus";
					public const string Station12spplus = "Station12spplus";
					public const string Station12sp = "Station12sp";
					public const string Station12 = "Station12";
					public const string Station30vip = "Station30vip";
					public const string StationTelecaster = "StationTelecaster";
					public const string StationTelecasterBus = "StationTelecasterBus";
					public const string StationPolycom = "StationPolycom";
					public const string Station130spplus = "Station130spplus";
					public const string StationPhoneApplication = "StationPhoneApplication";
					public const string AnalogAccess = "AnalogAccess";
					public const string DigitalAccessTitan1 = "DigitalAccessTitan1";
					public const string DigitalAccessTitan2 = "DigitalAccessTitan2";
					public const string DigitalAccessLennon = "DigitalAccessLennon";
					public const string AnalogAccessElvis = "AnalogAccessElvis";
					public const string ConferenceBridge = "ConferenceBridge";
					public const string ConferenceBridgeYoko = "ConferenceBridgeYoko";
					public const string H225 = "H225";
					public const string H323Phone = "H323Phone";
					public const string H323Trunk = "H323Trunk";
					public const string MusicOnHold = "MusicOnHold";
					public const string Pilot = "Pilot";
					public const string TapiPort = "TapiPort";
					public const string TapiRoutePoint = "TapiRoutePoint";
					public const string VoiceInbox = "VoiceInbox";
					public const string VoiceInboxAdmin = "VoiceInboxAdmin";
					public const string LineAnnunciator = "LineAnnunciator";
					public const string SoftwareMtpDixieland = "SoftwareMtpDixieland";
					public const string CiscoMediaServer = "CiscoMediaServer";
					public const string RouteList = "RouteList";
					public const string LoadSimulator = "LoadSimulator";
					public const string Ipste1 = "Ipste1";
					public const string MediaTerminationPoint = "MediaTerminationPoint";
					public const string MediaTerminationPointYoko = "MediaTerminationPointYoko";
					public const string MediaTerminationPointDixieland = "MediaTerminationPointDixieland";
					public const string MediaTerminationPointSummit = "MediaTerminationPointSummit";
					public const string MgcpStation = "MgcpStation";
					public const string MgcpTrunk = "MgcpTrunk";
					public const string RasProxy = "RasProxy";
					public const string Trunk = "Trunk";
					public const string Annunciator = "Annunciator";
					public const string MonitorBridge = "MonitorBridge";
					public const string Recorder = "Recorder";
					public const string MonitorBridgeYoko = "MonitorBridgeYoko";
					public const string UnknownMgcpGateway = "UnknownMgcpGateway";
					public const string Ipste2 = "Ipste2";
				}
			}

			public abstract class ConfigValueFormats
			{
				public const string Version	= "VersionFormat";
				public const string DeviceType	= "DeviceTypeFormat";
			}

			public abstract class DefaultValues
			{
				// Provider parameters
				public const int MaxBurst = 5;
				public const int InterBurstDelayMs = 1 * 1000;

				// Stack parameters
				public const int CallManagerPort = 2000;
				public const bool AdvertiseLowBitRateCodecs = false;
				public const int LingerSec = 2;
				public const int InitialTimerThreadpoolSize = 5;
				public const int MaxTimerThreadpoolSize = 15;
				public const int InitialSelectorActionThreadpoolSize = 5;
				public const int MaxSelectorActionThreadpoolSize = 15;
				public const int MinWaitForKeepaliveAfterRegisterSec = 1;
				public const int MaxWaitForKeepaliveAfterRegisterSec = 17;
				public const int WaitingForRegisterAckKeepaliveMs = 30 * 1000;
				public const int RetryTokenRequestMs = 5 * 1000;
				public const int StuckAlarmCount = 6;
				public const int CallManagerConnectRetries = 2;
				public const int SrstConnectRetries = 0;
				public const int KeepaliveJitterPercent = 20;
				public const int PadMs = 3 * 1000;
				public const int WaitingForUnregisterMs = 10 * 1000;
				public const int NoCallManagersDefinedMs = 60 * 1000;
				public const int RetryPrimaryMs = 10 * 1000;
				public const int MaxCallManagerListIterations = 4;
				public const int MinWaitForUnregisterSec = 30;
				public const int MaxWaitForUnregisterSec = 330;
				public const int RejectMs = 60 * 1000;
				public const int AckRetriesBeforeLockout = 3;
				public const int KeepaliveTimeoutsBeforeNoTokenRegister = 3;
				public const int UnregisterAckRetries = 1;
				public const int DevicePollJitterPercent = 50;
				public const int MaxLines = 42;
				public const int MaxFeatures = 40;
				public const int MaxServiceUrls = 40;
				public const int MaxSoftkeyDefinitions = 32;
				public const int MaxSoftkeySetDefinitions = 16;
				public const int MaxSpeeddials = 100;
				public const int CallEndMs = 1 * 1000;
				public const int CloseMs = 5 * 1000;
				public const int DevicePollMs = 10 * 1000;
				public const int LockoutMs = 10 * 60 * 1000;
				public const int NakToSynRetryMs = 250;
				public const int ConnectMs = 5 * 1000;
				public const int DefaultKeepaliveMs = 10 * 1000;
				public const string Version = ConfigEntries.Versions.Parche;
				public const string DeviceType = ConfigEntries.DeviceTypes.StationTelecasterMgr;
				public const bool LogCallVerbose = true;
				public const bool LogCallManagerVerbose = false;
				public const bool LogConnectionVerbose = false;
				public const bool LogDiscoveryVerbose = false;
				public const bool LogRegistrationVerbose = true;
				public const bool LogSystemVerbose = false;

				public const bool MohOption = true;
			}
			public abstract class Range
			{
				public abstract class Min
				{
					// Provider parameters
					public const int MaxBurst			= 1;
					public const int InterBurstDelayMs	= 0;

					// Stack parameters
					public const int CallManagerPort = 1024;
					public const int LingerSec = 0;
					public const int InitialTimerThreadpoolSize = 1;
					public const int MaxTimerThreadpoolSize = 1;
					public const int InitialSelectorActionThreadpoolSize = 1;
					public const int MaxSelectorActionThreadpoolSize = 1;
					public const int MinWaitForKeepaliveAfterRegisterSec = 1;
					public const int MaxWaitForKeepaliveAfterRegisterSec = 1;
					public const int WaitingForRegisterAckKeepaliveMs = 0;
					public const int RetryTokenRequestMs = 0;
					public const int StuckAlarmCount = 1;
					public const int CallManagerConnectRetries = 0;
					public const int SrstConnectRetries = 0;
					public const int KeepaliveJitterPercent = 0;
					public const int PadMs = 0;
					public const int WaitingForUnregisterMs = 0;
					public const int NoCallManagersDefinedMs = 0;
					public const int RetryPrimaryMs = 0;
					public const int MaxCallManagerListIterations = 1;
					public const int MinWaitForUnregisterSec = 0;
					public const int MaxWaitForUnregisterSec = 0;
					public const int RejectMs = 0;
					public const int AckRetriesBeforeLockout = 0;
					public const int KeepaliveTimeoutsBeforeNoTokenRegister = 0;
					public const int UnregisterAckRetries = 0;
					public const int DevicePollJitterPercent = 0;
					public const int MaxLines = 0;
					public const int MaxFeatures = 0;
					public const int MaxServiceUrls = 0;
					public const int MaxSoftkeyDefinitions = 0;
					public const int MaxSoftkeySetDefinitions = 0;
					public const int MaxSpeeddials = 0;
					public const int CallEndMs = 0;
					public const int CloseMs = 0;
					public const int DevicePollMs = 0;
					public const int LockoutMs = 0;
					public const int NakToSynRetryMs = 0;
					public const int ConnectMs = 0;
					public const int DefaultKeepaliveMs = 0;
				}
				public abstract class Max
				{
					// Provider parameters
					public const int MaxBurst			= int.MaxValue;
					public const int InterBurstDelayMs	= int.MaxValue;

					// Stack parameters
					public const int CallManagerPort = 32767;
					public const int LingerSec = int.MaxValue;
					public const int InitialTimerThreadpoolSize = int.MaxValue;
					public const int MaxTimerThreadpoolSize = int.MaxValue;
					public const int InitialSelectorActionThreadpoolSize = int.MaxValue;
					public const int MaxSelectorActionThreadpoolSize = int.MaxValue;
					public const int MinWaitForKeepaliveAfterRegisterSec = int.MaxValue;
					public const int MaxWaitForKeepaliveAfterRegisterSec = int.MaxValue;
					public const int WaitingForRegisterAckKeepaliveMs = int.MaxValue;
					public const int RetryTokenRequestMs = int.MaxValue;
					public const int StuckAlarmCount = int.MaxValue;
					public const int CallManagerConnectRetries = int.MaxValue;
					public const int SrstConnectRetries = int.MaxValue;
					public const int KeepaliveJitterPercent = 50;
					public const int PadMs = int.MaxValue;
					public const int WaitingForUnregisterMs = int.MaxValue;
					public const int NoCallManagersDefinedMs = int.MaxValue;
					public const int RetryPrimaryMs = int.MaxValue;
					public const int MaxCallManagerListIterations = int.MaxValue;
					public const int MinWaitForUnregisterSec = int.MaxValue;
					public const int MaxWaitForUnregisterSec = int.MaxValue;
					public const int RejectMs = int.MaxValue;
					public const int AckRetriesBeforeLockout = int.MaxValue;
					public const int KeepaliveTimeoutsBeforeNoTokenRegister = int.MaxValue;
					public const int UnregisterAckRetries = int.MaxValue;
					public const int DevicePollJitterPercent = 50;
					public const int MaxLines = int.MaxValue;
					public const int MaxFeatures = int.MaxValue;
					public const int MaxServiceUrls = int.MaxValue;
					public const int MaxSoftkeyDefinitions = int.MaxValue;
					public const int MaxSoftkeySetDefinitions = int.MaxValue;
					public const int MaxSpeeddials = int.MaxValue;
					public const int CallEndMs = int.MaxValue;
					public const int CloseMs = int.MaxValue;
					public const int DevicePollMs = int.MaxValue;
					public const int LockoutMs = int.MaxValue;
					public const int NakToSynRetryMs = int.MaxValue;
					public const int ConnectMs = int.MaxValue;
					public const int DefaultKeepaliveMs = int.MaxValue;
				}
			}
		}

		/// <summary>
		/// Directory number assigned to Device when it is not registered.
		/// </summary>
		/// <remarks>
		/// By convention, use "0" to indicate that the Device no longer
		/// has a directory number. This constant is not in the private Const
		/// class because it is the only one that needs to be public.
		/// </remarks>
		public const string NotADirectoryNumber = "0";

		/// <summary>
		/// The action name for MohDisabled. 
		/// </summary>
		/// 
		/// <remarks>
		/// It's used in p2p calls for one leg of the call to notify the other leg of the 
		/// call about MohDisabled.
		/// </remarks>

		public const string ActionMohDisabled = "MohDisabled";

		#endregion

		/// <summary>
		/// The SCCP stack through which the provider communicates with
		/// CallManagers.
		/// </summary>
		private SccpStack.SccpStack stack;

		/// <summary>
		/// Encapsulation of collection that contains entries for all devices.
		/// </summary>
		private Devices devices;

		/// <summary>
		/// Whether OnStartup has been called.
		/// </summary>
		/// <remarks>
		/// RefreshConfiguration is ineffectual until this is set to true.
		/// </remarks>
		private bool started = false;

		/// <summary>
		/// Address/port to use as an RTP "bit bucket" for when the protocol
		/// requires an address but we don't really have anywhere to send the
		/// stream.
		/// </summary>
		private readonly IPEndPoint bitBucket;

		/// <summary>
		/// Property whose value is the address/port to use as an RTP "bit
		/// bucket."
		/// </summary>
		public IPEndPoint BitBucket { get { return bitBucket; } }

		/// <summary>
		/// Type of delegate for determining whether the specified device meets
		/// a certain criteria.
		/// </summary>
		private delegate bool IsDeviceDelegate(ConfigData.DeviceInfo device,
			object obj);

		/// <summary>
		/// The collection that contains entries for all registered devices,
		/// indexed by directory number.
		/// </summary>
		private RegisteredDevices registeredDevices;

		/// <summary>
		/// The collection that contains entries for all Devices engaged in a
		/// call, indexed by call id.
		/// </summary>
		private readonly EngagedDevices engagedDevices;

		/// <summary>
		/// Controls the sending of OpenDeviceRequest and CloseDeviceRequest
		/// messages to the stack so that the CallManager is never overwhelmed.
		/// </summary>
		private readonly ProcessObject registrationThrottle;

		/// <summary>
		/// Whether we are in the process of shutting down the provider.
		/// </summary>
		private volatile bool shuttingDown;

		/// <summary>
		/// Port on which CallManagers listen for registrations.
		/// </summary>
		private int callManagerPort;

		#region Action Handlers (from the Telephony Manager)

		/// <summary>
		/// Handles the Music-On-Hold Media action from the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleUseMohMedia(long callId)
		{
			LogVerbose("Prv: HandleUseMohMedia({0})", callId);

			return devices.UseMohMedia(callId);
		}

		/// <summary>
		/// Handles when the Telephony Manager cannot find a handler--no script
		/// instance--for an event that the provider previously sent up to it.
		/// </summary>
		/// <param name="noHandlerAction">The no-handler action.</param>
		/// <param name="originalEvent">Original event for which the Telephony
		/// Manager cannot find a handler.</param>
        protected override void HandleNoHandler(Msg.ActionMessage noHandlerAction,
			Msg.EventMessage originalEvent)
        {
            log.Write(TraceLevel.Warning,
				"Prv: event was not handled: {0}", originalEvent.MessageId);

			LogVerbose("Prv: details of previous unhandled event: {0}",
					originalEvent.ToString());
		}

		/// <summary>Handles the MakeCall action from the Telephony Manager.</summary>
		/// <param name="callInfo">Call metadata</param>
		/// <returns>success</returns>
        protected override bool HandleMakeCall(OutboundCallInfo outCallInfo)
        {
			bool madeCall = false;

			LogVerbose("Prv: HandleMakeCall({0}, {1}, {2}, {3}, {4}, {5})",
				outCallInfo.CallId, outCallInfo.To, outCallInfo.From,
				outCallInfo.DisplayName, outCallInfo.IsPeerToPeer,
				outCallInfo.Caps);

			// Choose a device and create an outgoing call with it.
			string deviceName = GetDeviceNameFromRouteGroup(outCallInfo, true);
			if (deviceName == null)
			{
				log.Write(TraceLevel.Warning,
					"Prv: all devices associated with {0}:{1} are in use or not available; cannot make call",
					outCallInfo.AppName, outCallInfo.PartitionName);
			}
			else
			{
				madeCall = CreateOutgoingCall(deviceName, outCallInfo);
			}

			return madeCall;
        }

		/// <summary>
		/// Handles the barge action from the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="into">Directory number of device whose shared-line
		/// call we barge into.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleBarge(long callId, string into)
		{
			LogVerbose("Prv: HandleBarge({0}, {1})", callId, into);

			bool barged;

			// Choose a device and create an outgoing call with it.
			lock (devices.SyncRoot)
			{
				Device device = registeredDevices[into];
				if (device != null && !device.InUse)
				{
					// (I guess use the same directory number for all three
					// directory-number fields.)
					barged = CreateBargedCall(device.MacAddress, callId, into, into,
						into);
				}
				else
				{
					log.Write(TraceLevel.Error,
						"Prv: Barge failed. SCCP device {0} not available",
						into);

					barged = false;
				}
			}

			return barged;
		}

		/// <summary>
		/// Handles the accept-call action from the Telephony Manager.
		/// </summary>
		/// <remarks>
		/// Note that this is different from answering a call.
		/// </remarks>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleAcceptCall(long callId, bool p2p)
        {
			LogVerbose("Prv: HandleAcceptCall({0})", callId);

			return devices.AcceptCall(callId);
        }

		/// <summary>
		/// Handles the answer-call action from the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="displayName">Name to display for the caller. Ignored.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleAnswerCall(long callId, string displayName)
        {
			LogVerbose("Prv: HandleAnswerCall({0})", callId);

			return devices.AnswerCall(callId);
        }

		/// <summary>
		/// Handles the redirect-call action from the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number to which the call is being
		/// redirected.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleRedirectCall(long callId, string to)
        {
			LogVerbose("Prv: HandleRedirectCall({0})", callId);

			return devices.RedirectCall(callId);
        }

		/// <summary>
		/// Handles the reject-call action from the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleRejectCall(long callId)
        {
			LogVerbose("Prv: HandleRejectCall({0})", callId);

			return devices.RejectCall(callId);
		}

		/// <summary>
		/// Handles the set-media action from the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="rxIP">Incoming RTP IP address.</param>
		/// <param name="rxPort">Incoming RTP (UDP) port.</param>
		/// <param name="rxControlIP">Incoming RTCP IP address.
		/// Ignored.</param>
		/// <param name="rxControlPort">Incoming RTCP (UDP) port.
		/// Ignored.</param>
		/// <param name="rxCodec">Incoming codec.</param>
		/// <param name="rxFramesize">Incoming frame size in
		/// milliseconds.</param>
		/// <param name="txCodec">Outgoing codec. Ignored.</param>
		/// <param name="txFramesize">Outgoing frame size in
		/// milliseconds. Ignored.</param>
		/// <param name="caps">Media capabilities. Ignored.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleSetMedia(long callId, string rxIP,
			uint rxPort, string rxControlIP, uint rxControlPort,
			IMediaControl.Codecs rxCodec, uint rxFramesize,
			IMediaControl.Codecs txCodec, uint txFramesize,
			Msg.MediaCaps.MediaCapsField caps)
        {
			LogVerbose("Prv: HandleSetMedia({0}, {1}, {2}, {3}, {4}, {5}, {6})",
					callId, rxIP, rxPort, rxControlIP, rxControlPort, rxCodec,
					rxFramesize, txCodec, txFramesize, caps);

			bool handled;

			// When the IP and port are missing from SetMedia, this is a
			// special case. The provider calls it "ClearMedia." It (always?)
			// means place the call on hold.
			if (rxIP == null || rxPort == 0)
			{
				handled = devices.ClearMedia(callId);
			}
			else
			{
				// Combine IP address and port into an IPEndPoint then pass it
				// on to the device.
				IPEndPoint mediaAddr = null;
				try
				{
					mediaAddr =
						new IPEndPoint(IPAddress.Parse(rxIP), (int)rxPort);
				}
				catch
				{
					// Do nothing.
				}
				if (mediaAddr != null)
				{
					devices.SetMedia(callId, mediaAddr, rxCodec, rxFramesize);
					handled = true;
				}
				else
				{
					log.Write(TraceLevel.Error,
						"Prv: address, {0}:{1}, " +
						"could not be converted into IP address for callId, {2}; " +
						"media not set",
						rxIP, rxPort, callId);
					handled = false;
				}
			}

            return handled;
        }

		/// <summary>
		/// Handles the Hold action from Telephony Manager. It requests a Hold on the
		/// the device for the call.
		/// </summary>
		/// <param name="callId">the id for the call</param>
		/// <returns>whether the action is handled successfully</returns>
        protected override bool HandleHold(long callId)
        {
			LogVerbose("Prv: HandleHold({0})", callId);

			bool handled = devices.HandleHold(callId);
			return handled;
        }

		/// <summary>
		/// Handles the Resume action from Telephony Manager. It requests a Hold on the device
		/// for the call.
		/// </summary>
		/// <param name="callId">call identification</param>
		/// <param name="rxIP">Incoming RTP Ip, could be null</param>
		/// <param name="rxPort">Incoming RTP port</param>
		/// <param name="rxControlIP">Incoming RTCP Ip</param>
		/// <param name="rxControlPort">Incoming RTCP port</param>
		/// <returns></returns>
        protected override bool HandleResume(long callId, string rxIP, uint rxPort,
			string rxControlIP, uint rxControlPort)
        {
			LogVerbose("Prv: HandleResume({0}, {1}, {2}, {3}, {4})",
				callId, rxIP, rxPort, rxControlIP, rxControlPort);

			return devices.HandleResume(callId, rxIP, rxPort, rxControlIP, rxControlPort);
		}

		/// <summary>
		/// Handles the send-user-input (DTMF) action from the Telephony
		/// Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="digits">DTMF digits to send.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleSendUserInput(long callId, string digits)
        {
			LogVerbose("Prv: HandleSendUserInput({0}, {1})", callId, digits);

			return devices.SendUserInput(callId, digits);
        }

		/// <summary>
		/// Handles the hangup action from the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <returns>Whether the action was successfully handled.</returns>
		protected override bool HandleHangup(long callId)
		{
			LogVerbose("Prv: HandleHangup({0})", callId);

			return devices.HangupCall(callId);
		}

		protected override bool HandleHairpinAction(ActionBase action, long callId)
		{
			bool rc = false;

			int pos = action.Name.LastIndexOf('.');
			string sn;
			if (pos >= 0)
				sn = action.Name.Substring(pos+1);
			else
				sn = action.Name;

			switch (sn)
			{
				case ActionMohDisabled:
					rc = devices.HandleMohDisabled(callId);
					break;

				default:
					break;
			}

			return rc;
		}

        #endregion

		#region Stack Events (to the Telephony Manager)

		/// <summary>
		/// Handles Setup event from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">Setup message.</param>
        private void OnSetup(SccpStack.Device device, ClientMessage message)
        {
            Assertion.Check(device != null,
				"SccpStack: device is null in SetupEvent");

            Setup setupMessage = message as Setup;
			if (setupMessage == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: invalid or incomplete client message sent in SetupEvent; terminating call",
					device.MacAddress);
				devices.HangupCall(
					devices.GetCallId(device.MacAddress));
			}
			else
			{
				CreateIncomingCall(device, setupMessage);
			}
        }

		/// <summary>
		/// Handles Alerting event from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">Alerting message.</param>
		private void OnAlerting(SccpStack.Device device, ClientMessage message)
		{
			Assertion.Check(device != null,
				"SccpStack: device is null in AlertingEvent");

			long callId = devices.GetCallId(device.MacAddress);
			Alerting alertingMessage = message as Alerting;
			if (alertingMessage == null)
			{
				log.Write(TraceLevel.Warning,
					"Prv: {0}: invalid or incomplete client message sent in AlertingEvent; terminating call",
					device.MacAddress);
				devices.HangupCall(callId);
			}
			else
			{
				if (callId == 0)
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received Alerting for unknown call; ignored",
						device.MacAddress);
				}
				else
				{
					if (!devices.ReceivedAlerting(callId, alertingMessage))
					{
						log.Write(TraceLevel.Warning,
							"Prv: {0}: failed to process Alerting for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
		}

		/// <summary>
		/// Handles Connect event from SCCP stack.
		/// </summary>
		/// <remarks>
		/// For outbound calls.
		/// </remarks>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">Connect message.</param>
        private void OnConnect(SccpStack.Device device,
			ClientMessage message)
        {
			long callId = devices.GetCallId(device.MacAddress);
			Connect connectMessage = message as Connect;
			if (connectMessage == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: invalid or incomplete client message sent in Connect; terminating call",
					device.MacAddress);
				devices.HangupCall(callId);
			}
			else
			{
				if (callId == 0)
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received Connect for unknown call; ignored",
						device.MacAddress);
				}
				else
				{
					if (!devices.ReceivedConnect(callId, connectMessage))
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: failed to process Connect for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
		}

		/// <summary>
		/// Handles ConnectAck event from SCCP stack.
		/// </summary>
		/// <remarks>
		/// For inbound calls.
		/// </remarks>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">ConnectAck message.</param>
		private void OnConnectAck(SccpStack.Device device,
			ClientMessage message)
        {
			long callId = devices.GetCallId(device.MacAddress);
			ConnectAck connAckMessage = message as ConnectAck;
			if (connAckMessage == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: invalid or incomplete client message sent in ConnectAck; terminating call",
					device.MacAddress);
				devices.HangupCall(callId);
			}
			else
			{
				if (callId == 0)
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received ConnectAck for unknown call; ignored",
						device.MacAddress);
				}
				else
				{
					if (!devices.ReceivedConnectAck(callId))
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: failed to process ConnectAck for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
        }

		/// <summary>
		/// Handles StartTransmit event from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">StartTransmit message.</param>
		private void OnStartTransmit(SccpStack.Device device,
			ClientMessage message)
        {
            StartTransmit xmitMessage = message as StartTransmit;
			if (xmitMessage == null || xmitMessage.media == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: invalid or incomplete client message sent in StartTransmit; ignored",
					device.MacAddress);
			}
			else
			{
				long callId = devices.GetCallId(device.MacAddress);
				if (callId == 0)
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received StartTransmit for unknown call; ignored",
						device.MacAddress);
				}
				else
				{
					if (!devices.StartTransmit(callId, message))
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: failed to process StartTransmit for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
        }

		/// <summary>
		/// Handles StopTransmit event from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">StopTransmit message. Ignored.</param>
		private void OnStopTransmit(SccpStack.Device device,
			ClientMessage message)
		{
			long callId = devices.GetCallId(device.MacAddress);
			if (callId == 0)
			{
				if (recentlyEngagedDevices.Contains(device.MacAddress))
				{
					log.Write(TraceLevel.Info,
						"Prv: {0}: received StopTransmit for call that has already ended; ignored",
						device.MacAddress);
				}
				else
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received StopTransmit for unknown call; ignored",
						device.MacAddress);
				}
			}
			else
			{
				if (!devices.StopTransmit(callId))
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: failed to process StopTransmit for call {1}; ignored",
						device.MacAddress, callId);
				}
			}
		}

		/// <summary>
		/// Handles Release event from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">Release message. Ignored.</param>
		private void OnRelease(SccpStack.Device device,
			ClientMessage message)
        {
			long callId = devices.GetCallId(device.MacAddress);
			Release releaseMessage = message as Release;
			if (releaseMessage == null)
			{
				log.Write(TraceLevel.Warning,
					"Prv: {0}: invalid or incomplete client message sent in ReleaseEvent; ignored",
					device.MacAddress);
				devices.HangupCall(callId);
			}
			else
			{
				if (callId == 0)
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received Release for unknown call; ignored",
						device.MacAddress);
				}
				else
				{
					if (!devices.ReceivedRelease(callId, releaseMessage))
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: failed to process Release for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
        }

		/// <summary>
		/// Handles ReleaseComplete event from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">ReleaseComplete message. Ignored.</param>
		private void OnReleaseComplete(SccpStack.Device device,
			ClientMessage message)
		{
			long callId = devices.GetCallId(device.MacAddress);
			ReleaseComplete releaseCompleteMessage = message as ReleaseComplete;
			if (releaseCompleteMessage == null)
			{
				log.Write(TraceLevel.Warning,
					"Prv: {0}: invalid or incomplete client message sent in ReleaseCompleteEvent; terminating call",
					device.MacAddress);
				devices.HangupCall(callId);
			}
			else
			{
				if (callId == 0)
				{
					if (recentlyEngagedDevices.Contains(device.MacAddress))
					{
						log.Write(TraceLevel.Info,
							"Prv: {0}: received ReleaseComplete for call that has already ended; ignored",
							device.MacAddress);
					}
					else
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: received ReleaseComplete for unknown call; ignored",
							device.MacAddress);
					}
				}
				else
				{
					if (!devices.ReceivedReleaseComplete(callId, releaseCompleteMessage))
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: failed to process ReleaseComplete for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
		}

		/// <summary>
		/// Handles OpenReceiveRequest event from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">OpenReceiveRequest message.</param>
		private void OnOpenReceiveRequest(SccpStack.Device device,
			ClientMessage message)
		{
			OpenReceiveRequest orrMessage = message as OpenReceiveRequest;
			if (orrMessage == null || orrMessage.media == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: invalid or incomplete client message sent in OpenReceiveRequest; ignored",
					device.MacAddress);
			}
			else
			{
				long callId = devices.GetCallId(device.MacAddress);
				if (callId == 0)
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received OpenReceiveRequest for unknown call; ignored",
						device.MacAddress);
				}
				else
				{
					if (!devices.ReceivedOpenReceiveRequest(callId, orrMessage))
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: failed to process OpenReceiveRequest for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
		}

		/// <summary>
		/// Handles Digits event (incoming DTMF) from SCCP stack.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">Digits message.</param>
		private void OnDigit(SccpStack.Device device, ClientMessage message)
		{
			Digits digits = message as Digits;
			if (digits == null || digits.digits == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: invalid or incomplete client message sent in Digits; ignored",
					device.MacAddress);
			}
			else
			{
				long callId = devices.GetCallId(device.MacAddress);
				if (callId == 0)
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: received Digits for unknown call; ignored",
						device.MacAddress);
				}
				else
				{
					if (!devices.ReceivedDigits(callId, digits))
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: failed to process Digits for call: {1}; ignored",
							device.MacAddress, callId);
					}
				}
			}
		}

		/// <summary>
		/// Handles StartTone event from SCCP stack.
		/// </summary>
		/// <remarks>
		/// If StartTone for Hold tone, send signal to devices.
		/// </remarks>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">StartTone message.</param>
		private void OnStartTone(SccpStack.Device device,
			SccpMessage message)
		{
			long callId = devices.GetCallId(device.MacAddress);
			if (callId != 0)
			{
				StartTone startTone = message as StartTone;
				if (startTone != null)
				{
					// Ignore if not Hold.
					if (startTone.tone == SccpStack.Tone.Hold)
					{
						if (!devices.StartHoldTone(callId))
						{
							log.Write(TraceLevel.Error,
								"Prv: {0}: failed to process StartTone for call: {1}; ignored",
								device.MacAddress, callId);
						}
					}
				}
				else
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: expected but received message other than StartTone for call: {1}; ignored",
						device.MacAddress, callId);
				}
			}
		}

		/// <summary>
		/// Handles StopTone event from SCCP stack.
		/// </summary>
		/// <remarks>
		/// StopTone received so set tell device. Used to detect when Hold tone
		/// stops.
		/// </remarks>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">StopTone message. Ignored.</param>
		private void OnStopTone(SccpStack.Device device, SccpMessage message)
		{
			long callId = devices.GetCallId(device.MacAddress);
			if (callId != 0)
			{
				if (!devices.StopTone(callId))
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: failed to process StopTone for call: {1}; ignored",
						device.MacAddress, callId);
				}
			}
		}

		/// <summary>
		/// Handles CallState event from SCCP stack.
		/// </summary>
		/// <remarks>
		/// Save SCCP callId (different from the TM callId) from CallState.
		/// We'll need it if we Barge.
		/// </remarks>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">CallState message.</param>
		private void OnCallState(SccpStack.Device device, SccpMessage message)
		{
			Device providerDevice = devices[device.MacAddress];
			if (providerDevice != null)
			{
				CallState callState = message as CallState;
				if (callState != null)
				{
					// Only save the call id from CallRemoteMultiline.
					if (callState.callState ==
						CallState.State.CallRemoteMultiline)
					{
						providerDevice.LastCallRemoteMultilineCallId =
							callState.CallId;
					}
				}
				else
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: expected but received message other than CallState for device {1}; ignored",
						device.MacAddress, providerDevice);
				}
			}
		}

		private void OnCallInfo(SccpStack.Device device, SccpMessage message)
		{
			CallInfo ci = message as CallInfo;
			if (ci == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: invalid or incomplete client message sent in CallInfo; ignored",
					device.MacAddress);
				return;
			}

			long callId = devices.GetCallId(device.MacAddress);
			if (callId != 0)
			{
				if (!devices.CallInfo(callId, ci))
				{
					log.Write(TraceLevel.Error,
						"Prv: {0}: failed to process CallInfo for call: {1}; ignored",
						device.MacAddress, callId);
				}
			}
		}

		#endregion

        #region Private Helpers

		/// <summary>
		/// Creates an IncomingCall, adds the device to the EngagedDevices
		/// collection, and sends up an IncomingCall event to the Telephony
		/// Manager.
		/// </summary>
		/// <param name="device">Device on which this message was sent
		/// from the stack.</param>
		/// <param name="message">Setup message.</param>
		private void CreateIncomingCall(SccpStack.Device device, Setup setup)
		{
			Device providerDevice = devices[device.MacAddress];
			if (providerDevice == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: no device found; incoming call not created",
					device.MacAddress);
			}
			else
			{
				if (providerDevice.InUse)
				{
					// Only complain if this is an unexpected
					// occurrence--receiving Setup during a call.
					if (!providerDevice.Call.SetupExpectedDuringCall)
					{
						LogVerbose("Prv: {0}: did not create incoming call",
								device.MacAddress);
					}
				}
				else
				{
					long callId = callIdFactory.GenerateCallId();
					string to = setup.calledPartyNumber;
					string from = setup.callingPartyNumber;
					string originalTo = setup.originalCalledPartyNumber;
					string displayName = setup.callingPartyName;

					providerDevice.CreateIncomingCall(this, callId, to, from,
						originalTo, displayName, log);

					IncomingCall(callId, to, from, originalTo, displayName);
				}
			}
		}

		/// <summary>
		/// Creates an OutgoingCall, adds the device to the EngagedDevices
		/// collection, and sends up a GotCapabilities event to the
		/// Telephony Manager.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="outCallInfo">Call metadata from app/TM action.</param>
		/// <returns>Whether the call was created.</returns>
		public bool CreateOutgoingCall(string macAddress, OutboundCallInfo outCallInfo)
		{
			bool created = false;

			Device device = devices[macAddress];
			if (device == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: no device found (call id: {1}); outgoing call not created",
					macAddress, outCallInfo.CallId);
			}
			else if (device.InUse)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: device in use; could not create outgoing call {1}",
					macAddress, outCallInfo.CallId);
			}
			else
			{
				device.CreateOutgoingCall(this, outCallInfo, log);

                // Jump past initial state to the wait-for-first-media
                // state since we don't want to actually place the call
                // until we have media from the app.
                created = device.CallTrigger(
                    (int)Call.EventType.WaitForFirstMedia);
                if (!created)
                {
                    log.Write(TraceLevel.Error,
                        "Prv: {0}: trigger WaitForFirstMedia failed (call id {1})",
                        macAddress, outCallInfo.CallId);

                    device.RemoveCall();
                }

                if(outCallInfo.RxAddr == null)
                {
                    GotCapabilities(outCallInfo.CallId, null);
                }
                else
                {
                    // We've got everything we need, place the call
                    devices.SetMedia(device.Call.CallId, outCallInfo.RxAddr, IMediaControl.Codecs.Unspecified, 0);
                }
			}

			return created;
		}

		/// <summary>
		/// Moves Call to another device.
		/// </summary>
		/// <param name="fromMacAddress">MAC address of Device from which Call is moved.</param>
		/// <param name="toMacAddress">MAC address of Device to which Call is moved.</param>
		/// <returns>Whether the call was moved.</returns>
		public bool MoveCall(string fromMacAddress, string toMacAddress)
		{
			bool moved = false;

			Device fromDevice = devices[fromMacAddress];
			if (fromDevice == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: no device found for from MAC address; cannot move call",
					fromMacAddress);
			}
			else
			{
				if (!fromDevice.InUse)
				{
					log.Write(TraceLevel.Warning,
						"Prv: {0}: from device not in use; cannot move call",
						fromMacAddress);
				}
				else
				{
					Device toDevice = devices[toMacAddress];
					if (toDevice == null)
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: no device found for to MAC address; cannot move call",
							toMacAddress);
					}
					else
					{
						if (toDevice.InUse)
						{
							log.Write(TraceLevel.Error,
								"Prv: {0}: to device in use; cannot move call",
								toMacAddress);
						}
						else
						{
							moved = Device.MoveCall(fromDevice, toDevice);
						}
					}
				}
			}

			return moved;
		}

		/// <summary>
		/// Creates a BargedCall, adds the device to the EngagedDevices
		/// collection, and sends up a GotCapabilities event to the Telephony
		/// Manager.
		/// </summary>
		/// <param name="macAddress">MAC address.</param>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		/// <returns>Whether the call was created.</returns>
		public bool CreateBargedCall(string macAddress, long callId, string to,
			string from, string originalTo)
		{
			bool created = false;

			Device device = devices[macAddress];
			if (device == null)
			{
				log.Write(TraceLevel.Error,
					"Prv: {0}: no device found (call id: {1}); barged call not created",
					macAddress, callId);
			}
			else
			{
				if (device.InUse)
				{
					LogVerbose("Prv: {0}: did not create barged call {1}",
							macAddress, callId);
				}
				else
				{
					device.CreateBargedCall(this, callId, to, from, originalTo,
						log);

					GotCapabilities(callId, null);

					// Jump past initial state to the wait-for-first-media
					// state since we don't want to actually place the call
					// until we have media from the app.
					created = device.CallTrigger(
						(int)Call.EventType.WaitForFirstMedia);
					if (!created)
					{
						log.Write(TraceLevel.Error,
							"Prv: {0}: trigger WaitForFirstMedia failed (call id {1})",
							macAddress, callId);

						device.RemoveCall();
					}
				}
			}

			return created;
		}

		/// <summary>
		/// Finds a device in the Call Route Group that is not being used and
		/// returns its name.
		/// </summary>
		/// <param name="outCallInfo">Call metadata from app/TM action.</param>
		/// <param name="nextDevicePool">Whether to start the search on the
		/// next or current device pool of the Call Route Group.</param>
		/// <returns>Name of next available-device or null if none found.</returns>
		internal string GetDeviceNameFromRouteGroup(OutboundCallInfo outCallInfo,
			bool nextDevicePool)
		{
			string deviceName = null;

			// Lock because we don't want two threads trying to traverse and
			// then--especially--update dp.DeviceIndex at the same time.
			lock (outCallInfo)
			{
				// Loop until we find an available-device MAC address or we run
				// out of device pools (aka, members).
				for (CrgMember member = nextDevicePool ?
						 outCallInfo.RouteGroup.GetNextMember() :
						 outCallInfo.RouteGroup.GetCurrentMember();
					member != null;
					member = outCallInfo.RouteGroup.GetNextMember())
				{
					SccpDevicePool dp = member as SccpDevicePool;
					if (dp != null)
					{
						uint deviceIndex;
						deviceName = GetDeviceNameFromList(dp.DeviceNames,
							outCallInfo.DeviceIndex == 0 ? 0 : outCallInfo.DeviceIndex + 1,
							out deviceIndex);
						if (deviceName != null)
						{
							// Remember where we last found a device so we'll
							// know where to start next time.
							outCallInfo.DeviceIndex = deviceIndex;

							// Leave loop and return available-device MAC address.
							break;
						}
					}
					else
					{
						log.Write(TraceLevel.Warning,
							"Prv: invalid CRG member detected for {0}:{1}; checking next member",
							outCallInfo.AppName, outCallInfo.PartitionName);
					}
				}
			}

			return deviceName;
		}

		/// <summary>
		/// Returns the name of the first available device from the list of
		/// MAC addresses starting at the provided index or null if none found.
		/// </summary>
		/// <remarks>"Available" means is registered with a CallManager and is
		/// not already in a call.</remarks>
		/// <param name="deviceNames">Collection of MAC addresses from which to
		/// choose.</param>
		/// <param name="startIndex">Where to start the search from (was where
		/// we left off last time).</param>
		/// <param name="deviceIndex">Index in device-name collection where we
		/// found the device.</param>
		/// <returns>Next available-device MAC address or null.</returns>
		private string GetDeviceNameFromList(StringCollection deviceNames,
			uint startIndex, out uint deviceIndex)
		{
			string deviceName = null;

            for (deviceIndex = startIndex; deviceIndex < deviceNames.Count;
				++deviceIndex)
            {
				string deviceNameCandidate = deviceNames[(int)deviceIndex];
                Device device = devices[deviceNameCandidate];
                if (device != null && device.IsRegistered && !device.InUse)
                {
                    deviceName = deviceNameCandidate;
					break;
                }
            }

            return deviceName;
		}

		/// <summary>
		/// Creates a stack device for a provider device to communicate with its
		/// CallManagers.
		/// </summary>
		/// <returns>SCCP stack device abstraction.</returns>
        private SccpStack.Device CreateSession()
        {
            SccpStack.Device device = stack.CreateDevice();
			if (device != null)
			{
				// Register callbacks for the high-level, SCCP client events
				// and low-level, SCCP-message events in which we are
				// interested.
				device.SetupEvent += new ClientMessageHandler(OnSetup);
				device.AlertingEvent += new ClientMessageHandler(OnAlerting);
				device.ConnectEvent += new ClientMessageHandler(OnConnect);
				device.ConnectAckEvent +=
					new ClientMessageHandler(OnConnectAck);
				device.OpenReceiveRequestEvent +=
					new ClientMessageHandler(OnOpenReceiveRequest);
				device.StartTransmitEvent +=
					new ClientMessageHandler(OnStartTransmit);
				device.StopTransmitEvent +=
					new ClientMessageHandler(OnStopTransmit);
				device.ReleaseEvent += new ClientMessageHandler(OnRelease);
				device.ReleaseCompleteEvent +=
					new ClientMessageHandler(OnReleaseComplete);
				device.DigitEvent += new ClientMessageHandler(OnDigit);
				device.StartToneEvent +=
					new ClientSccpMessageHandler(OnStartTone);
				device.StopToneEvent +=
					new ClientSccpMessageHandler(OnStopTone);
				device.CallStateEvent += new ClientSccpMessageHandler(OnCallState);

				device.CallInfoEvent += new ClientSccpMessageHandler(OnCallInfo);
			}

            return device;
        }

        #endregion

		#region Public proxy methods for protected "SendX" methods in CallControlProviderBase

		/// <summary>
		/// Passes media capabilities up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="remoteMediaCaps">Media capabilities of the remote
		/// device.</param>
		public void GotCapabilities(long callId, MediaCapsField remoteMediaCaps)
		{
			LogVerbose("Prv: SendGotCapabilities({0}, {1})", callId, remoteMediaCaps);

			base.SendGotCapabilities(callId, remoteMediaCaps);
		}

		/// <summary>
		/// Passes the incoming-call event up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		/// <param name="originalTo">Original directory number being called in
		/// case of a forwarded call.</param>
		/// <param name="displayName">Display name of caller.</param>
		public void IncomingCall(long callId, string to, string from,
			string originalTo, string displayName)
		{
			LogVerbose("Prv: SendIncomingCall({0}, {1}, {2}, {3}, {4})",
					callId, to, from, originalTo, displayName);

			base.SendIncomingCall(callId, callId.ToString(), to, from, originalTo, false, displayName);
		}

		/// <summary>
		/// Passes the call-established event up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="to">Directory number being called.</param>
		/// <param name="from">Directory number from which this call
		/// originates.</param>
		public void CallEstablished(long callId, string to, string from)
		{
			LogVerbose("Prv: SendCallEstablished({0}, {1}, {2})", callId, to, from);

			base.SendCallEstablished(callId, to, from);
		}

		/// <summary>
		/// Passes the media-established event up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="txIP">Outgoing RTP IP address.</param>
		/// <param name="txPort">Outgoing RTP (UDP) port.</param>
		/// <param name="txControlIP">Outgoing RTCP IP address.</param>
		/// <param name="txControlPort">Outgoing RTCP (UDP) port.</param>
		/// <param name="rxCodec">Incoming codec.</param>
		/// <param name="rxFramesize">Incoming frame size in
		/// milliseconds.</param>
		/// <param name="txCodec">Outgoing codec.</param>
		/// <param name="txFramesize">Outgoing franme size in
		/// milliseconds.</param>
		public void MediaEstablished(long callId,
			string txIP, uint txPort, string txControlIP, uint txControlPort,
			IMediaControl.Codecs rxCodec, uint rxFramesize,
			IMediaControl.Codecs txCodec, uint txFramesize)
		{
			LogVerbose("Prv: SendMediaEstablished({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
					callId, txIP, txPort, txControlIP, txControlPort,
					rxCodec, rxFramesize, txCodec, txFramesize);

			base.SendMediaEstablished(callId,
				txIP, txPort, txControlIP, txControlPort,
				rxCodec, rxFramesize, txCodec, txFramesize);
		}

		/// <summary>
		/// Passes the media-changed event up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="txIP">Outgoing RTP IP address.</param>
		/// <param name="txPort">Outgoing RTP (UDP) port.</param>
		public void MediaChanged(long callId, string txIP, uint txPort)
		{
			LogVerbose("Prv: SendMediaChanged({0}, {1}, {2})", callId, txIP, txPort);

			base.SendMediaChanged(callId, txIP, txPort);
		}

		/// <summary>
		/// Passes the media-changed event up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="txIP">Outgoing RTP IP address.</param>
		/// <param name="txPort">Outgoing RTP (UDP) port.</param>
		/// <param name="txControlIP">Outgoing RTCP IP address.</param>
		/// <param name="txControlPort">Outgoing RTCP (UDP) port.</param>
		/// <param name="rxCodec">Incoming codec.</param>
		/// <param name="rxFramesize">Incoming frame size in
		/// milliseconds.</param>
		/// <param name="txCodec">Outgoing codec.</param>
		/// <param name="txFramesize">Outgoing franme size in
		public void MediaChanged(long callId, string txIP, uint txPort,
			string txControlIP, uint txControlPort,
			IMediaControl.Codecs rxCodec, uint rxFramesize,
			IMediaControl.Codecs txCodec, uint txFramesize)
		{
			LogVerbose("Prv: SendMediaChanged({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8})",
					callId, txIP, txPort, txControlIP, txControlPort,
					rxCodec, rxFramesize, txCodec, txFramesize);

			base.SendMediaChanged(callId,
				txIP, txPort, txControlIP, txControlPort,
				rxCodec, rxFramesize, txCodec, txFramesize);
		}

		/// <summary>
		/// Request remote hold
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		public void RemoteHold(long callId)
		{
			LogVerbose("Prv: SendRemoteHold {0}", callId);
			base.SendRemoteHold(callId);
		}

		/// <summary>
		/// Request remote resume
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="txIP">Outgoing RTP IP address.</param>
		/// <param name="txPort">Outgoing RTP (UDP) port.</param>
		/// <param name="txControlIP">Outgoing RTCP IP</param>
		/// <param name="txControlPort">Outgoing RTCP port</param>
		public void RemoteResume(long callId, string txIP, uint txPort, string txControlIP, uint txControlPort)
		{
			LogVerbose("Prv: SendRemoteResume {0}", callId);
			base.SendRemoteResume(callId, txIP, txPort, txControlIP, txControlPort);
		}

		/// <summary>
		/// Passes the digits event (incoming DTMF) up to the Telephony
		/// Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		/// <param name="digits">DTMF digits received.</param>
		public void GotDigits(long callId, string digits)
		{
			LogVerbose("Prv: SendGotDigits({0}, {1})", callId, digits);

			base.SendGotDigits(callId, digits);
		}

		/// <summary>
		/// Passes the hangup event up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		public void Hangup(long callId)
		{
			LogVerbose("Prv: SendHangup({0})", callId);

			base.SendHangup(callId);
		}

		/// <summary>
		/// Passes the hairpinaction event up to the Telephony Manager.
		/// </summary>
		/// <param name="callId">Uniquely identifies this call for use between
		/// the Telephony Manager and the provider.</param>
		public void HairpinAction(long callId, string actionName, Field[] fields)
		{
			LogVerbose("Prv: SendHairpinAction({0})", callId);
			base.SendHairpinAction(callId, actionName, fields);
		}

		#endregion

		#region LogVerbose signatures
		/// <summary>
		/// Logs Verbose diagnostic if logVerbose set to true.
		/// </summary>
		/// <param name="message">String to log.</param>
		protected void LogVerbose(string text)
		{
			if (logCallVerbose)
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
			if (logCallVerbose)
			{
				log.Write(TraceLevel.Verbose, text, args);
			}
		}
		#endregion
	}

	#region Internal registration messages

	/// <summary>
	/// Represents an internal registration message, which is either an
	/// OpenDeviceRequest or a CloseDeviceRequest.
	/// </summary>
	public class InternalRegistrationMessage
	{
		/// <summary>
		/// Constructs an InternalRegistrationMessage with the common
		/// parameter, device.
		/// </summary>
		/// <param name="device">Device to which this message applies.</param>
		public InternalRegistrationMessage(Device device)
		{
			this.device = device;
		}

		protected Device device;

		public virtual void Send() { }
	}

	/// <summary>
	/// Represents an OpenDeviceRequest message within the provider, which
	/// causes the provider to request that the stack register the indicated
	/// device.
	/// </summary>
	/// <remarks>Note that this is distinct from the SccpStack class of the
	/// same name.</remarks>
	public class OpenDeviceRequest : InternalRegistrationMessage
	{
		/// <summary>
		/// Constructs an OpenDeviceRequest message.
		/// </summary>
		/// <param name="device">Device to which this message applies.</param>
		/// <param name="callManagerAddresses">List of CallManager addresses
		/// with which the device is to register.</param>
		/// <param name="srstAddresses">List of SRST addresses.</param>
		public OpenDeviceRequest(Device device,
			ArrayList callManagerAddresses, ArrayList srstAddresses)
			: base(device)
		{
			this.callManagerAddresses = callManagerAddresses;
			this.srstAddresses = srstAddresses;
		}

		/// <summary>
		/// List of CallManager addresses with which the device is to register.
		/// </summary>
		public ArrayList callManagerAddresses;

		/// <summary>
		/// List of SRST IPEndPoints which this device may use for recovery.
		/// </summary>
		/// <remarks>(SRST stands for Survivable Remote Site Telephony.)</remarks>
		public ArrayList srstAddresses;

		/// <summary>
		/// Sends an SccpStack.OpenDeviceRequest message to the stack.
		/// </summary>
		public override void Send()
		{
			// Make sure the device hasn't been nuked before we are asked to
			// send the message.
			if (device != null)
			{
				device.Send(new SccpStack.OpenDeviceRequest(
					device.MacAddress, callManagerAddresses, srstAddresses));
			}
		}
	}

	/// <summary>
	/// Represents a CloseDeviceRequest message within the provider, which
	/// causes the provider to request that the stack unregister the indicated
	/// device.
	/// </summary>
	/// <remarks>Note that this is distinct from the SccpStack class of the
	/// same name.</remarks>
	public class CloseDeviceRequest : InternalRegistrationMessage
	{
		/// <summary>
		/// Constructs a CloseDeviceRequest message.
		/// </summary>
		/// <param name="device">Device to which this message applies.</param>
		public CloseDeviceRequest(Device device) : base(device) { }

		/// <summary>
		/// Sends an SccpStack.CloseDeviceRequest message to the stack.
		/// </summary>
		public override void Send()
		{
			// Make sure the device hasn't been nuked before we are asked to
			// send the message.
			if (device != null)
			{
				device.Send(new SccpStack.CloseDeviceRequest());
			}
		}
	}

	#endregion
}
