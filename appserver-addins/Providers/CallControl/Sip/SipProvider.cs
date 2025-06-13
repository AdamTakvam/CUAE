//#define SIP_DRIVER

using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;
using System.Text;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Messaging.MediaCaps;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Core.ConfigData.CallRouteGroups;
using Metreos.Utilities;

namespace Metreos.CallControl.Sip
{
	[ProviderDecl("SIP Call Control Provider")]
	public class SipProvider : CallControlProviderBase
	{
        #region Constants
        
        private abstract class Consts
        {
			public static char[] FromFieldSeparator = new char[]{'@'};

            public const string DisplayName     = "SIP Provider";
            public const string ExtNamespace    = "Metreos.Providers.Sip";
			public const string NullIp			= "0.0.0.0";

            public const int MediaTimeout       = 5000;
            public const int RingingTimeout     = 3000;

            public const int Port				= 9500;

            public const int MorgueSize         = 100;
			

			public abstract class ConfigValueNames
			{
				public const string DefaultOutBoundFromNumber = "DefaultOutboundFromNumber";	//the number used to populate From field for an outbound call
				public const string MinRegistrationPort	= "MinRegistrationPort";				//starting port number for device registration
				public const string MaxRegistrationPort	= "MaxRegistrationPort";				//ending port nubmer for device registration
				public const string SipTrunkIp		= "SIPTrunkIP";								//sip trunk ip
				public const string SipTrunkPort	= "SIPTrunkPort";							//sip trunk port
				public const string ServiceLogLevel = "ServiceLogLevel";						//stack log levle
				public const string LogTimingStat = "LogTimingStat";							//whether to log timing stats or not
			}

			public abstract class DefaultValues
			{
				public const int MinRegistrationPort	= 1024;
				public const int MaxRegistrationPort	= 65535;
				public const int SipPort				= 5060;
				public const int ServiceLogLevel        = 2;    // Warning
				public const bool LogTimingStat		= false;
			}
        }

		public abstract class DefaultValues
		{
			public const uint MaxCallsPerDevice = 1;
		}

		public enum Source : uint
		{
			Unspecified = 0,
			Remote      = 1,
			Local       = 2
		}

		#endregion

		#region Member variables

        /// <summary>Abstraction for Sip service communication</summary>
        private SipProxy proxy;

        /// <summary>Table of DeviceInfo objects</summary>
        /// 
		private Hashtable devices;

		/// <summary>
		/// Table of CallInfo objects
		/// </summary>
        private CallInfoMap	calls;

		/// <summary>
		/// CallManager device configuration requestor.
		/// It sends a tftp request to get the initial config file
		/// and then it parses the file to get the directory number
		/// </summary>
		/// 
		private CcmDirectoryNumberRequestor ccmDnrequestor;

		#endregion


        #region Construction/Init/Startup/Refresh/Shutdown/Cleanup

		/// <summary>
		/// The constructor
		/// </summary>
		/// <param name="configUtility">configuration utility</param>
		/// <param name="callIdFactory">call id factory</param>
        public SipProvider(IConfigUtility configUtility, ICallIdFactory callIdFactory)
            : base(typeof(SipProvider), Consts.DisplayName, IConfig.ComponentType.Cisco_SIP_DevicePool,
            configUtility, callIdFactory)
        {
			devices = Hashtable.Synchronized(new Hashtable());
            this.calls = new CallInfoMap(Consts.MorgueSize, log);
            this.calls.ConfigUtility = configUtility;
			proxy = CreateSipProxy();
			ccmDnrequestor = new CcmDirectoryNumberRequestor(configUtility, log, proxy);
        }

		/// <summary>
		/// It initializes the provider. It creates the configuration parameter entries.
		/// </summary>
		/// <returns>true if successful</returns>
        protected override bool DeclareConfig(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            configItems = new ConfigEntry[7];

            configItems[0] = new ConfigEntry(Consts.ConfigValueNames.DefaultOutBoundFromNumber, null, null,
                "Default From number for outbound call", IConfig.StandardFormat.String, true);

			configItems[1] = new ConfigEntry(Consts.ConfigValueNames.SipTrunkIp, null, null,
				"Application Server will place all calls from this address. It should match the IP used for SIP trunk in Communication Manager. If you plan to support SIP phones, please add a second IP address to the server and populate this field with the IP address for the SIP trunk as configured in Communications Manager. If not specified, it will default to an IP address configured on the server.", 
				IConfig.StandardFormat.IP_Address, false);

			configItems[2] = new ConfigEntry(Consts.ConfigValueNames.SipTrunkPort, null, Consts.DefaultValues.SipPort,
				"SIP Trunk port for outbound call. It should match the port used for SIP Trunk in Communications Manager.",
				IConfig.StandardFormat.Number,
				true);
			
			configItems[3] = new ConfigEntry(Consts.ConfigValueNames.MinRegistrationPort, 
				Consts.ConfigValueNames.MinRegistrationPort, 
				Consts.DefaultValues.MinRegistrationPort,
				"Minimum TCP port number to use for registration with SIP server", 
				Consts.DefaultValues.MinRegistrationPort,
				Consts.DefaultValues.MaxRegistrationPort, 
				true);

			configItems[4] = new ConfigEntry(Consts.ConfigValueNames.MaxRegistrationPort,
				Consts.ConfigValueNames.MaxRegistrationPort, 
				Consts.DefaultValues.MaxRegistrationPort,
				"Maximum TCP port number to use for registration with SIP server", 
				Consts.DefaultValues.MinRegistrationPort,
				Consts.DefaultValues.MaxRegistrationPort, 
				true);

			configItems[5] = new ConfigEntry(Consts.ConfigValueNames.ServiceLogLevel, 
				Consts.ConfigValueNames.ServiceLogLevel, 
				Consts.DefaultValues.ServiceLogLevel, 
				"SIP service log level. 0=Off, 1=Error, 2=Warning, 3=Info, 4=Verbose",
				0, //min log level
				4, //max log level
				true);        

			configItems[6] = new ConfigEntry(Consts.ConfigValueNames.LogTimingStat, 
				Consts.ConfigValueNames.LogTimingStat, 
				Consts.DefaultValues.LogTimingStat, 
				"Set it to true to enable timing statistics",
				IConfig.StandardFormat.Bool,
				true);

            // No extensions
            extensions = null;

			return true;
        }

		/// <summary>
		/// Intial startup
		/// </summary>
        protected override void OnStartup()
		{
			// Connect to Sip service
			proxy.Startup(new IPEndPoint(IPAddress.Loopback, Consts.Port));

			//start up the DN requestor thread
			ccmDnrequestor.Start();

            // Call Control namespace registration
            base.RegisterNamespace();
            base.RegisterSecondaryProtocolType(IConfig.ComponentType.SIP_Trunk);
			base.RegisterSecondaryProtocolType(IConfig.ComponentType.IETF_SIP_DevicePool);
        }

		/// <summary>
		/// The callback to retrieve all the sip devices
		/// </summary>
		/// <returns></returns>
        private Core.ConfigData.SipDeviceInfo[] OnGetDevices()
        {
            Assertion.Check(devices != null, "Device table is null in Sip provider");

			ArrayList devList = new ArrayList();
			foreach(SipDeviceInfo dInfo in devices.Values)
			{
				// Theoretically, we wouldn't be interested in the running ones
				//  but in reality, the status can get hosed under some error conditions
				if (dInfo.Status == IConfig.Status.Enabled_Stopped ||
					dInfo.Status == IConfig.Status.Enabled_Running)
				{
					devList.Add(dInfo);
				}
			}

			Core.ConfigData.SipDeviceInfo[] devs = new Core.ConfigData.SipDeviceInfo[devList.Count];
			devList.CopyTo(devs, 0);
			return devs;

        }
  
		/// <summary>
		/// It is called whenever the configuration is changed. It gives the provider the chance
		/// to notify the stack of the changes.
		/// </summary>
        protected override void RefreshConfiguration()
        {
			//stack configuration parameters
			int i;
			string s;
			bool paramChanged = false;
			//DefaultFromOutboundNumber is sent to stack every time it needs it, so 
			//there is no need to check for change
			proxy.DefaultFromOutboundNumber = Convert.ToString(GetConfigValue(Consts.ConfigValueNames.DefaultOutBoundFromNumber));
			i = Convert.ToInt32(GetConfigValue(Consts.ConfigValueNames.MinRegistrationPort));
			paramChanged = paramChanged || (i != proxy.MinRegistrationPort);
			proxy.MinRegistrationPort = i;

			i = Convert.ToInt32(GetConfigValue(Consts.ConfigValueNames.MaxRegistrationPort));
			paramChanged = paramChanged || (i != proxy.MaxRegistrationPort);
			proxy.MaxRegistrationPort = i;

			s = Convert.ToString(GetConfigValue(Consts.ConfigValueNames.SipTrunkIp));
            if(s == null || s.Length == 0) //not defined
            {
                //use current local ip instead
                s = IpUtility.GetIPAddresses()[0].ToString();
            }

            paramChanged = paramChanged || !s.Equals(proxy.SipTrunkIp);
			proxy.SipTrunkIp = s;

			i = Convert.ToInt32(GetConfigValue(Consts.ConfigValueNames.SipTrunkPort));
			paramChanged = paramChanged || (i != proxy.SipTrunkPort);
			proxy.SipTrunkPort = i;

			i = Convert.ToInt32(GetConfigValue(Consts.ConfigValueNames.ServiceLogLevel));
			paramChanged = paramChanged || ( i !=proxy.ServiceLogLevel);
			proxy.ServiceLogLevel = i;

			bool b = Convert.ToBoolean(GetConfigValue(Consts.ConfigValueNames.LogTimingStat));
			paramChanged = paramChanged || ( b !=proxy.LogTimgingStat);
			proxy.LogTimgingStat = b;

			//config data has been read in and ready to be consumed
			proxy.ConfigDataReady.Set();

			if (paramChanged && proxy.Connected)
			{
				proxy.SendParameterChanged();
			}

            // See if any new devices have been added.
            ArrayList configDevices = GetSipDevicesFromDb();
            if(configDevices == null)
                configDevices = new ArrayList();

            StringCollection configDevNames = new StringCollection();

            foreach(Core.ConfigData.SipDeviceInfo dInfo in configDevices)
            {
                if(devices.Contains(dInfo.Key) == false)
                {
                    devices.Add(dInfo.Key, dInfo);

                    if(proxy.Connected)
                    {
                        log.Write(TraceLevel.Info, "Registering new device: " + dInfo.Name);
						if (dInfo.Type == IConfig.DeviceType.CiscoSip)	//dont register siptrunk
						{
							//for cisco devices without DN, add a request to get the DN through tftp
							if (dInfo.DirectoryNumber == null || 
								dInfo.DirectoryNumber.Length == 0 || 
								dInfo.DirectoryNumber.Equals("0"))
								ccmDnrequestor.AddRequest(dInfo);
							else	//else register it with stack
								proxy.SendRegister(dInfo);
						}
						else if (dInfo.Type == IConfig.DeviceType.IetfSip)	//always register with stack for Ietf devices
							proxy.SendRegister(dInfo);
						//don't need to register for trunk
                    }
                }

                configDevNames.Add(dInfo.Key);
            }

            // See if any devices have been removed
			SipDeviceInfo[] dInfos = new SipDeviceInfo[devices.Count];
			devices.Values.CopyTo(dInfos, 0);
			foreach (SipDeviceInfo di in dInfos)
			{
                if(configDevNames.Contains( di.Key ) == false)
                {
                    devices.Remove(di.Key);
                    
					//if the device has been removed
                    if(proxy.Connected)
                    {
                        log.Write(TraceLevel.Info, "Unregistering device: " + di.Name);
						//unregister only if the device is Ietf device or
						//it is a cisco device with DN
						if (di.Type == IConfig.DeviceType.IetfSip ||
							(di.Type == IConfig.DeviceType.CiscoSip && 
								di.DirectoryNumber != null && 
								di.DirectoryNumber.Length > 0 &&
								!di.DirectoryNumber.Equals("0")) )	//dont need to unregister SipTrunk
							proxy.SendUnregister(di);
                    }
                }
            }
        }
		
		/// <summary>
		/// Internal startup 
		/// </summary>
		private void InternalStartup()
		{
			// Make a new proxy object
//			proxy = CreateSipProxy();
			proxy.Startup(new IPEndPoint(IPAddress.Loopback, Consts.Port));
		}

		/// <summary>
		/// It creates the proxy to stack and initializes all the delegates/
		/// </summary>
		/// <returns></returns>
        private SipProxy CreateSipProxy()
        {
            SipProxy proxy = new SipProxy(log);
            proxy.onServiceGone = new VoidDelegate(StopAllDevices);
            proxy.onGetDevices = new GetDevicesDelegate(OnGetDevices);

            proxy.onCallEstablished = new OnCallEstablishedDelegate(OnCallEstablished);
            proxy.onAnswered = new OnCallAnsweredDelegate(OnAnswered);
            proxy.onError = new OnErrorDelegate(OnError);
            proxy.onMakeCallAck = new CallNoticeDelegate(OnMakeCallAck);
            proxy.onHangup = new OnHangupDelegate(OnHangup);
            proxy.onIncomingCall = new OnIncomingCallDelegate(OnIncomingCall);
            proxy.onRinging = new CallNoticeDelegate(OnRinging);
            proxy.onMediaEstablished = new OnMediaEstablishedDelegate(OnMediaEstablished);
            proxy.onStatusUpdate = new OnStatusUpdateDelegate(OnStatusUpdate);
//            proxy.onCallInitiated = new OnCallInitiatedDelegate(PropOnCallInitiated);
//            proxy.onCallInactive = new OnCallInactiveDelegate(PropOnCallInactive);
            proxy.onGotDigits = new OnGotDigitsDelegate(OnGotDigits);
			proxy.onGotCapabilities = new OnGotCapabilitiesDelegate(OnGotCapabilities);
			proxy.onStatusUpdate = new OnStatusUpdateDelegate(OnStatusUpdate);
			proxy.onReInvite = new OnReInviteDelegate(OnReInvite);
			proxy.onRequestDirectoryNumber = new OnRequestDirectoryNumberDelegate(OnRequestDirectoryNumber);
			proxy.onResetDirectoryNumber = new OnResetDirectoryNumberDelegate(OnResetDirectoryNumber);

			return proxy;
        }

		/// <summary>
		/// It marks all device to be stopped.
		/// </summary>
        private void StopAllDevices()
        {
            // Change the status of all device from Enabled_Running to Enabled_Stopped
            foreach(DeviceInfo dInfo in devices.Values)
            {
				if (dInfo.Status == IConfig.Status.Enabled_Running)
					dInfo.Status = IConfig.Status.Enabled_Stopped;
            }
        }

		/// <summary>
		/// Callback function for provider shutdown
		/// </summary>
        protected override void OnShutdown()
        {
            InternalShutdown(true);
        }

		/// <summary>
		/// Internal shutdown helper. It shuts down the connection to stack.
		/// Also it shuts down the CCM directory number requestor.
		/// And finally it clears all the call cache and device cache.
		/// 
		/// </summary>
		/// <param name="final">true if it is a final shutdown</param>
        private void InternalShutdown(bool final)
        {
            StopAllDevices();

			if (proxy != null)
			{
				proxy.ConfigDataReady.Reset();
				proxy.Shutdown();
				proxy = null;
			}

			if (ccmDnrequestor != null )
			{
				ccmDnrequestor.Shutdown();
			}

			if(final)
			{
				calls.Clear();
				devices.Clear();
			}
        }

		/// <summary>
		/// It cleans up all the cache and threads.
		/// </summary>
        public override void Cleanup()
        {
            if(proxy != null)
            {
                proxy.Dispose();
                proxy = null;
            }
            
			if (calls != null)
			{
				calls.Clear();
				calls = null;
			}

            if(devices != null)
            {
                devices.Clear();
                devices = null;
            }

			if (ccmDnrequestor != null)
			{
				ccmDnrequestor = null;
			}

            base.Cleanup();
        }

        #endregion

        #region CallControl action handlers

		/// <summary>
		/// Handler for making outbound calls. It finds a free device from the device pool 
		/// or trunk and creates a call. Then it forwards the request to stack.
		/// </summary>
		/// <param name="callInfo">all the parameters needed for outbound call</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleMakeCall(OutboundCallInfo callInfo)
        {
            return MakeCall(callInfo);
        }

		/// <summary>
		/// helper for making calls.
		/// </summary>
		/// <param name="callInfo"></param>
		/// <returns>true if the request is send successfully to the stack</returns>
        private bool MakeCall(OutboundCallInfo callInfo)
        {
            CrgMember member = callInfo.RouteGroup.GetNextMember();
			if(member == null)
			{
				log.Write(TraceLevel.Warning, "All SIP devices associated with {0}->{1} are in use or not available",
					callInfo.AppName, callInfo.PartitionName);
				return false;
			}

            // Handle device pools
			Core.ConfigData.SipDeviceInfo dv = null;
			SipDevicePool dp = member as SipDevicePool;
			SipTrunk tr = member as SipTrunk;
			if(dp != null)
			{
				dv = GetFreeDevice(dp);         
				if(dv == null)
				{
					return MakeCall(callInfo);
				}
			}
			else if (tr != null)
			{
				dv = GetFreeDevice(tr);
				// If this device is no good, try the next
				if(dv == null)
					return MakeCall(callInfo);
			}
			else
			{
				Assertion.Check(false, "Invalid CallRouteGroup member: " + member);
			}

            // The from field must be a valid line on the device.
			// For sip device, use its DirectoryNumber for from if it is not empty.
			// For all other types of devices and empty DirectoryNumber, pass in null
			// so the stack can make a choice.
			string from = null;
			if (dv.Type != IConfig.DeviceType.SipTrunk && dv.DirectoryNumber != null && dv.DirectoryNumber.Length>0)
				from = dv.DirectoryNumber;

            CallInfo cInfo = calls.AddCall(null, callInfo.CallId,
				callInfo.To, 
				from,
				CallDirection.Outbound,
				dv.DomainName);

			if(cInfo == null)
			{
				log.Write(TraceLevel.Error, "Could not make new call for {0}", callInfo.CallId);
				return false;
			}

            cInfo.State = CallState.PendingOutbound;
			cInfo.RxIp = callInfo.RxAddr != null ? callInfo.RxAddr.Address.ToString() : null;
			cInfo.RxPort = callInfo.RxAddr != null ? callInfo.RxAddr.Port : 0;

			if (proxy.LogTimgingStat)
				log.Write(TraceLevel.Info, "Start making call: {0}", cInfo.CallId);

			bool success =proxy.SendMakeCall(cInfo, dv, callInfo.Caps);

			if(success == false)
			{
				log.Write(TraceLevel.Error, "Failed to place outbound call (ID: {0})", callInfo.CallId);	
				calls.RemoveCall(cInfo.CallId);
				base.SendCallSetupFailed(callInfo.CallId, ICallControl.EndReason.Unknown);
			}

			return true;
        }

		/// <summary>
		/// Handler for accepting calls. It simply forwards the request from TM to stack.
		/// </summary>
		/// <param name="callId">identifies the call to be accepted</param>
		/// <param name="p2p">whethere it is p2p</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleAcceptCall(long callId, bool p2p)
        {
      /*      CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to accept a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received AcceptCall for non-existent call: " + callId);

                return false;
            }

            if(cInfo.Error)
                return false;

            return proxy.SendAcceptCall(cInfo.CallId, cInfo.StackCallId);
        */
			return true;
		}

		/// <summary>
		/// Hanlder for redirecting calls. Depending on the call state, it either sends
		/// a blind transfer request or redirect request to stack.
		/// </summary>
		/// <param name="callId">identifies the call to be redirected</param>
		/// <param name="to">where the call to be redirected</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleRedirectCall(long callId, string to)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to redirect a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received RedirectCall for non-existent call: " + callId);

                return false;
            }

			bool success = false;
			
			lock(cInfo.CallLock)
			{
				if(cInfo.Error)
					return false;

				if(cInfo.State == CallState.Init)
				{
					success = proxy.SendRedirect(cInfo.StackCallId, to, cInfo.DeviceDomain);
					calls.RemoveCall(callId);
				}
				else if(cInfo.State == CallState.Active)
				{
					// They meant BlindTransfer, don't play stupid
					success = proxy.SendBlindTransfer(cInfo.StackCallId, to, cInfo.DeviceDomain);
					calls.RemoveCall(callId);
				}
				else
				{
					log.Write(TraceLevel.Warning, "Call is not in a state where it call be redirected: {0} (state: {1})",
						callId.ToString(), cInfo.State.ToString());
				}
			}

            return success;
        }

		/// <summary>
		/// Handler for blind transfer. It sends a redirect request if the call is 
		/// in init state. Otherwise it forwards blind transfer request to stack
		/// directly.
		/// </summary>
		/// <param name="callId">the call to be blind transferred</param>
		/// <param name="to">where to tansfer to</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleBlindTransfer(long callId, string to)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to transfer a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received BlindTransfer for non-existent call: " + callId);

                return false;
            }

			bool success = false;
			
			if(cInfo.Error)
                return false;
			
			lock(cInfo.CallLock)
			{
				if(cInfo.State == CallState.Init)
				{
					// They meant Redirect, don't play stupid
					success = proxy.SendRedirect(cInfo.StackCallId, to, cInfo.DeviceDomain);
					calls.RemoveCall(callId);
				}
				else if(cInfo.State == CallState.Active)
				{
					success = proxy.SendBlindTransfer(cInfo.StackCallId, to, cInfo.DeviceDomain);
					calls.RemoveCall(callId);
				}
				else
				{
					log.Write(TraceLevel.Warning, "Call is not in a state where it call be transfered: {0} (state: {1})",
						callId.ToString(), cInfo.State.ToString());
				}
			}

            return success;
        }

		/// <summary>
		/// Handler for answering calls. It forwards the request to stack.
		/// </summary>
		/// <param name="callId">identifies the call to be answered</param>
		/// <param name="displayName">display name for the call</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleAnswerCall(long callId, string displayName)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to answer a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received AnswerCall for non-existent call: " + callId);

                return false;
            }

            if(cInfo.Error)
                return false;
            
			lock(cInfo.CallLock)
			{
				if(cInfo.State == CallState.Init || cInfo.State == CallState.Ringing)
				{
					if (proxy.LogTimgingStat)
						log.Write(TraceLevel.Info, "SendAnswerCall: {0}", callId);
					cInfo.State = CallState.Answered;
					return proxy.SendAnswerCall(cInfo.CallId, cInfo.StackCallId, cInfo.RxIp, 
						(int)cInfo.RxPort, cInfo.Codec, cInfo.Framesize,
						cInfo.MediaCaps);
				}

				log.Write(TraceLevel.Error, "Cannot answer call {0}, invalid state: {1}",
					callId, cInfo.State.ToString());
			}

			return false;
        }

		/// <summary>
		/// Handler for setting media. It checks for media information and determine whether
		/// TM is requesting a hold or just media change. It then forwards the change to the
		/// stack.
		/// </summary>
		/// <param name="callId">identifies the call</param>
		/// <param name="rxIP">IP for receiving media</param>
		/// <param name="rxPort">port for receiving media</param>
		/// <param name="rxControlIP">control IP for receiving media</param>
		/// <param name="rxControlPort">control port for receiving media</param>
		/// <param name="rxCodec">codec for receiving media</param>
		/// <param name="rxFramesize">framesize for receiving media</param>
		/// <param name="txCodec">codec for sending media</param>
		/// <param name="txFramesize">framesize for sending media</param>
		/// <param name="caps">media capabilities</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleSetMedia(long callId, string rxIP, uint rxPort, string rxControlIP, 
            uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, 
            uint txFramesize, MediaCapsField caps)
        {
			log.Write(TraceLevel.Verbose, "HandleSetMedia-New media info: callId={0}, rxIp={1}, rxPort={2}, rxCodec={3}, txCodec={4}, framesize={5}",
						callId, rxIP, rxPort, rxCodec, txCodec, txFramesize);

            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received SetMedia for non-existent call: " + callId);
                return false;
            }

			lock(cInfo.CallLock)
			{
				if(cInfo.Error)
					return false;

				log.Write(TraceLevel.Verbose, "HandleSetMedia-Old media info: callId={0}, rxIp={1}, rxPort={2}, rxCodec={3}, framesize={4}, state={5}",
					callId, cInfo.RxIp, cInfo.RxPort, cInfo.Codec, cInfo.Framesize, cInfo.State);

				if (rxCodec == IMediaControl.Codecs.Unspecified && caps == null)
				{
					//there is no usable information in this SetMedia message,
					//just ignore it
					log.Write(TraceLevel.Verbose, "Received SetMedia without any usable information for call: {0}. Ignore it.", callId);
					return true;
				}

				if (cInfo.State != CallState.ReInviting && rxIP == cInfo.RxIp && rxPort == cInfo.RxPort && txCodec == cInfo.Codec && 
					txFramesize == cInfo.Framesize)
				{
					//there is no change in media info in this SetMedia message,
					//and the caller doesn't expect any response from provider
					//just ignore it
					log.Write(TraceLevel.Verbose, "Received SetMedia without any change from previous setting for call: {0}. Ignore it.", callId);
					return true;
				}

				bool putOnHold = (cInfo.RxIp != null && rxIP == null);
				bool resume = (cInfo.State == CallState.OnHold && putOnHold);

				// Save media info
				cInfo.RxIp = rxIP;
				cInfo.RxPort = (int)rxPort;
				cInfo.Codec = txCodec;
				cInfo.Framesize = txFramesize;
				cInfo.MediaCaps = caps;

				//SIP uses symmetric codec setting, so just use the txCodec to set RxCodec 
				//if they are not the same
				if (txCodec != rxCodec || txFramesize != rxFramesize)
				{
					SendMediaEstablished(cInfo.CallId, cInfo.TxIp, (uint)cInfo.TxPort, 
						cInfo.TxIp, (uint)cInfo.TxPort,	txCodec, txFramesize, txCodec, txFramesize);
					//				cInfo.MediaEstablished = true;
				}

				if (resume)
				{
					cInfo.State = CallState.Active;
					return proxy.SendResume(cInfo.CallId, cInfo.StackCallId, cInfo.RxIp, (uint)cInfo.RxPort);
				}
				else if (putOnHold)
				{
					//a hold request
					if (cInfo.State != CallState.OnHold)
					{
						cInfo.State = CallState.OnHold;
						return proxy.SendHold(cInfo.CallId, cInfo.StackCallId);
					}
					else
					{
						log.Write(TraceLevel.Info, "The call {0} has been onhold. Don't request again.", cInfo.CallId);
						return true;
					}
				}
					// Send new Rx address to Sip service if call is established
				else if (cInfo.MediaEstablished || cInfo.IsMediaInfoComplete())
				{
					if (proxy.LogTimgingStat)
						log.Write(TraceLevel.Info, "SendSetMedia: {0}", callId);
					return proxy.SendSetMedia(cInfo.CallId, cInfo.StackCallId, cInfo.RxIp, (int)cInfo.RxPort, txCodec, txFramesize, caps);
				}
				else
					return true;
			}
        }

		/// <summary>
		/// Handler for holding a call
		/// </summary>
		/// <param name="callId">identifies the call</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleHold(long callId)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received Hold for non-existent call: " + callId);
                return false;
            }

            if(cInfo.Error)
                return false;

			lock(cInfo.CallLock)
			{
				cInfo.State = CallState.OnHold;
				proxy.SendHold(cInfo.CallId, cInfo.StackCallId);
			}
			
            return true;
        }

		/// <summary>
		/// Handler for resuming a call
		/// </summary>
		/// <param name="callId">identifies the call</param>
		/// <param name="rxIP">IP for receiving media</param>
		/// <param name="rxPort">port for receiving media</param>
		/// <param name="rxControlIP">control IP for receiving media</param>
		/// <param name="rxControlPort">control port for receiving media</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleResume(long callId, string rxIP, uint rxPort, string rxControlIP, uint rxControlPort)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received Resume for non-existent call: " + callId);
                return false;
            }

            if(cInfo.Error)
                return false;

			lock(cInfo.CallLock)
			{
				cInfo.RxIp = rxIP;
				cInfo.RxPort = (int) rxPort;
				
				cInfo.State = CallState.Active;
				proxy.SendResume(callId, cInfo.StackCallId, cInfo.RxIp, (uint)cInfo.RxPort);
			}
            return true;
        }

		/// <summary>
		/// Handler for Use media on hold 
		/// </summary>
		/// <param name="callId">the call id</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleUseMohMedia(long callId)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received UseMusicOnHoldMedia for non-existent call: " + callId);
                return false;
            }

			lock(cInfo.CallLock)
			{
				cInfo.State = CallState.Active;
				return proxy.SendUseMohMedia(cInfo.CallId, cInfo.StackCallId);
			}
        }

		/// <summary>
		/// Handler for rejecting a call
		/// </summary>
		/// <param name="callId">identifies the call to be rejected</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleRejectCall(long callId)
        {
            return HandleTerminateCall(callId);
        }

		/// <summary>
		/// Handler for hanging up the call
		/// </summary>
		/// <param name="callId">identifies the call to be hung up</param>
		/// <returns>true if the request is send successfully to the stack</returns>
		protected override bool HandleHangup(long callId)
        {
            return HandleTerminateCall(callId);
        }

		/// <summary>
		/// Handler for sending user input
		/// </summary>
		/// <param name="callId">identifies the call to be rejected</param>
		/// <param name="digits">digits to be sent</param>
		/// <returns>true if the request is send successfully to the stack</returns>
        protected override bool HandleSendUserInput(long callId, string digits)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo == null)
            {
                if(calls.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to send digits on a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received SendUserInput for non-existent call: " + callId);

                return false;
            }

            return proxy.SendUserInput(cInfo.StackCallId, digits);
        }

		/// <summary>
		/// Handler for terminating a call
		/// </summary>
		/// <param name="callId">identifies the call to be terminated</param>
		/// <returns>true if the request is send successfully to the stack</returns>
		private bool HandleTerminateCall(long callId)
        {
            CallInfo cInfo = calls.GetCall(callId);
            if(cInfo != null)
            {
				lock(cInfo.CallLock)
				{
					if(cInfo.State == CallState.Init)
						proxy.SendRejectCall(cInfo.StackCallId);
					else
						proxy.SendHangup(cInfo.StackCallId);

				}
				
				calls.RemoveCall(callId);
            }
            return true;
        }

		/// <summary>
		/// Default handler for messages from TM
		/// </summary>
		/// <param name="noHandlerAction">Action requested</param>
		/// <param name="originalEvent">event message</param>
		protected override void HandleNoHandler(ActionMessage noHandlerAction, EventMessage originalEvent)
		{
			log.Write(TraceLevel.Info, "{0} event was not handled", originalEvent.MessageId); 

			if(originalEvent.MessageId == ICallControl.Events.INCOMING_CALL)
			{
				long callId = Convert.ToInt64(originalEvent[ICallControl.Fields.CALL_ID]);
				HandleTerminateCall(callId);
			}
		}

		#endregion

        #region CallControl event senders

		/// <summary>
		/// It handles the error message from stack.
		/// </summary>
		/// <param name="dn">Directory number related to the error</param>
		/// <param name="cid">call id for the error</param>
		/// <param name="reason">the reason for the error</param>
		/// <param name="message">additional message for the error</param>
		/// <param name="msgType">error message type</param>
		/// <param name="msgField">message field</param>
        private void OnError(string dn, long cid, SipProxy.FailReason reason, 
            string message, SipProxy.MsgType msgType, SipProxy.MsgField msgField)
        {
            string title = "<Missing ID info>";
            if(dn != null && dn != String.Empty)
            {
	            title = String.Format("Call {0}:{1} ", dn, cid);
            }
            else
                title = String.Format("Call {0} ", cid);

            string errorMsg = String.Format("{0} encountered an error ({1}): {2}",
                title, reason.ToString(), message != null ? message : "<no description>");

            // Terminate the call
            CallInfo cInfo = calls.GetCall(cid);
            if(cInfo != null)
            {
				lock(cInfo.CallLock)
				{
					cInfo.ErrorMessage = errorMsg;

					if(cInfo.State == CallState.PendingOutbound)
					{
						calls.RemoveCall(cid);
						base.SendCallSetupFailed(cInfo.CallId, cInfo.EndReason);
					}
					else if(reason == SipProxy.FailReason.GeneralFailure)
					{
						calls.RemoveCall(cInfo.CallId);
						base.SendHangup(cInfo.CallId, ICallControl.EndReason.InternalError);
					}
				}
            }
            
            log.Write(TraceLevel.Error, errorMsg);
        }

		/// <summary>
		/// It handles the makecallack message from stack. It updates the stack call id for the call.
		/// </summary>
		/// <param name="callId"></param>
		/// <param name="stackCallId"></param>
        private void OnMakeCallAck(long callId, string stackCallId)
        {
            // Do nothing
			if (proxy.LogTimgingStat)
				log.Write(TraceLevel.Info, "OnMakeCallAck: {0}", callId);

			CallInfo cInfo = calls.GetCall(callId);
			if(cInfo == null)
			{
				log.Write(TraceLevel.Warning, "Received MakeCall ACK for non-existent call: " + callId);
				proxy.SendHangup(stackCallId);
				return;
			}
			else
			{
//				cInfo.StackCallId = stackCallId;
				calls.UpdateStackCallId(cInfo, stackCallId);
				log.Write(TraceLevel.Verbose, "Received MakeCallACK for " + callId);
			}
        }

		/// <summary>
		/// It handles the ringing event. It updates the call state to be ringing.
		/// </summary>
		/// <param name="cid"></param>
		/// <param name="nullparam"></param>
        private void OnRinging(long cid, string nullparam)
        {
            CallInfo cInfo = calls.GetCall(cid);
            if(cInfo == null)
            {
				CallInfo ended = calls.RecentlyEndedCall(cid);
                if(ended != null)
                {
                    log.Write(TraceLevel.Warning, "Received Ringing for non-existent call: " + cid);
                    proxy.SendHangup(ended.StackCallId);
                }
                return;
            }
            
			lock(cInfo.CallLock)
			{
				if(cInfo.Error)
					return;
				else if(cInfo.State == CallState.Init)
					cInfo.State = CallState.Ringing;
				else if(cInfo.State == CallState.Answered)
				{
					proxy.SendAnswerCall(cInfo.CallId, cInfo.StackCallId, cInfo.RxIp, 
						(int)cInfo.RxPort, cInfo.Codec, cInfo.Framesize,
						cInfo.MediaCaps);
				}
			}
        }

		/// <summary>
		/// It updates the status in the db for the device. It also updates the directory number
		/// for the device. If the status is Enabled_Stopped for cisco device, it addes a request 
		/// to get its directory number from CCM.
		/// </summary>
		/// <param name="dn"></param>
		/// <param name="deviceName"></param>
		/// <param name="status"></param>
        private void OnStatusUpdate(string dn, string deviceName, IConfig.Status status)
        {
            if(status == IConfig.Status.Unspecified || status == IConfig.Status.Disabled)
            {
				if (proxy.LogTimgingStat)
					log.Write(TraceLevel.Info, "Sip service attempting to set device {0}'s status to: {1}", 
						dn, status.ToString());
                return;
            }
            else if(status == IConfig.Status.Disabled_Error)
            {
                log.Write(TraceLevel.Warning, "Device '{0}' has encountered an unrecoverable error.", dn);
            }
            else if(status == IConfig.Status.Enabled_Running)
            {
                log.Write(TraceLevel.Info, "Device '{0}' registered successfully.", dn);
            }

			string[] dns = dn.Split(Consts.FromFieldSeparator);
			Core.ConfigData.SipDeviceInfo device = null;
			if (deviceName != null)
				device = (Core.ConfigData.SipDeviceInfo) devices[deviceName];
			else
				device = (Core.ConfigData.SipDeviceInfo) devices[dns[0]];
			if (device == null)	//try to search for the dn
				device = LookupDeviceFromDN(dns[0]);

			if (device != null)
			{
				device.Status = status;
				device.DirectoryNumber = dns[0];
				configUtility.UpdateDeviceStatus(device.Name, device.Type, status);
				configUtility.SetDirectoryNumber(device.Name, device.Type, device.DirectoryNumber);

				if (status == IConfig.Status.Enabled_Stopped && device.Type == IConfig.DeviceType.CiscoSip)	
					//failed to register, try to get new config from ccm
				{
					ccmDnrequestor.AddRequest(device);
				}

			}
        }

		/// <summary>
		/// It updates the call state to be active for outbound calls. For other calls,
		/// it sends CallChanged message to TM.
		/// </summary>
		/// <param name="cid">identifies the call</param>
        private void OnCallEstablished(long cid)
        {
            CallInfo cInfo = calls.GetCall(cid);
            if(cInfo == null)
            {
				CallInfo ended = calls.RecentlyEndedCall(cid);
                if(ended != null)
                {
                    log.Write(TraceLevel.Warning, "Received CallEstablished message for non-existent call: " + cid);
                    proxy.SendHangup(ended.StackCallId);
                }
                return;
            }

			lock(cInfo.CallLock)
			{
				if(cInfo.State == CallState.Active)
				{
					base.SendCallChanged(cInfo.CallId, cInfo.To, cInfo.From);
				}
				else
				{
					// Ignore this message for outbound calls and wait for Answered instead.
					base.SendCallEstablished(cInfo.CallId, cInfo.To, cInfo.From);
					cInfo.State = CallState.Active;
				}
			}
        }

		/// <summary>
		/// It updates the stack call id for the call. If the media info is still incomplete, it notifies
		/// TM with the new media info. It also send MediaEstablished message to TM. Finally it notifies 
		/// TM with CallEstablished.
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="stackCallId">stack call id for the call</param>
		/// <param name="from">where the call is from</param>
		/// <param name="to">where the call is to</param>
		/// <param name="originalTo">where the call is originally to</param>
		/// <param name="txIp">IP for media transmission</param>
		/// <param name="txPort">Port for media transmission</param>
		/// <param name="caps">capabilities for the call</param>
        private void OnAnswered(long cid, string stackCallId, string from, string to, string originalTo, string txIp, int txPort, MediaCapsField caps)
        {
			if (proxy.LogTimgingStat)
				log.Write(TraceLevel.Info, "OnAnswered: {0}", cid);

            CallInfo cInfo = calls.GetCall(cid);
            if(cInfo == null)
            {
				CallInfo ended = calls.RecentlyEndedCall(cid);
                if(ended != null)
                {
                    log.Write(TraceLevel.Warning, "Received Answered message for non-existent call: " + cid);
                    proxy.SendHangup(ended.StackCallId);
                }
                return;
            }

			lock(cInfo.CallLock)
			{
				cInfo.To = to;

				// This message should only ever arrive for outbound calls
				//if(cInfo.Direction == CallDirection.Outbound)
				{
					calls.UpdateStackCallId(cInfo, stackCallId);	//just now we have the full dialog id from stack
					if (caps != null && caps.Count > 0)
					{
						cInfo.TxIp = txIp;
						cInfo.TxPort = txPort;

						if (!cInfo.IsMediaInfoComplete())	//only send GotCaps when the media info is incomplete
							SendGotCapabilities(cid, caps);

						IMediaControl.Codecs codec = IMediaControl.Codecs.Unspecified;
						uint fms = 0;
						if (caps != null && caps.Count == 1)	//codec has been selected
						{
							//take the first codec in caps and use that as the negotiated codec
							IEnumerator it = caps.GetEnumerator();
							it.MoveNext();

							codec = (IMediaControl.Codecs) ((DictionaryEntry) it.Current).Key;
							fms = caps.GetFramesizes(codec)[0];

							cInfo.Codec = codec;
							cInfo.Framesize = fms;
						}

						SendMediaEstablished(cInfo.CallId, Consts.NullIp.Equals(cInfo.TxIp) ? null : cInfo.TxIp,
							Consts.NullIp.Equals(cInfo.TxIp) ? 0 : (uint)cInfo.TxPort, 
							cInfo.TxIp, (uint)cInfo.TxPort,
							codec, fms, 
							codec, fms);
/*
						//if (cInfo.RxIp != null)	//media info has been fully negotiated
						{
							//take the first codec in caps and use that as the negotiated codec
							IEnumerator it = caps.GetEnumerator();
							it.MoveNext();

							IMediaControl.Codecs codec = (IMediaControl.Codecs) ((DictionaryEntry) it.Current).Key;
							uint[] fms = caps.GetFramesizes(codec);
							SendMediaEstablished(cInfo.CallId, Consts.NullIp.Equals(cInfo.TxIp) ? null : cInfo.TxIp,
								Consts.NullIp.Equals(cInfo.TxIp) ? 0 : (uint)cInfo.TxPort, 
								cInfo.TxIp, (uint)cInfo.TxPort,
								codec, fms[0], codec, fms[0]);
						}								
*/					}
					
					base.SendCallEstablished(cInfo.CallId, cInfo.To, cInfo.From);

					cInfo.State = CallState.Active;
				}
			}
		}

		/// <summary>
		/// It notifies TM of MediaEstablished. It could result in a MediaChanged message
		/// if the media is already established for the call.
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="codec">codec for the call</param>
		/// <param name="framesize">frame size for the call</param>
		/// <param name="txIP">IP for media transmission</param>
		/// <param name="txPort">Port for media transmission</param>
        private void OnMediaEstablished(long cid, IMediaControl.Codecs codec, 
            uint framesize, string txIP, ushort txPort)
        {
            CallInfo cInfo = calls.GetCall(cid);
            if(cInfo == null)
            {
				CallInfo ended = calls.RecentlyEndedCall(cid);
                if(ended != null)
                {
                    log.Write(TraceLevel.Warning, "Received MediaEstablished message for non-existent call: " + cid);
                    proxy.SendHangup(ended.StackCallId);
                }
                return;
            }

			lock(cInfo.CallLock)
			{
				if(cInfo.MediaEstablished)
				{
					base.SendMediaChanged(cInfo.CallId, txIP, txPort);
				}
				else
				{
					base.SendMediaEstablished(cInfo.CallId, txIP, txPort, null, 0, codec, framesize, codec, framesize);
					cInfo.MediaEstablished = true;
				}
			}
        }

		/// <summary>
		/// It creates and adds the call to cache and notifies TM about the call.
		/// If there is caps along with the call, it notifies TM about its caps and
		/// sends MediaEstablished message to TM as well.
		/// </summary>
		/// <param name="stackCallId">identifies the call</param>
		/// <param name="dn">directory number for the call</param>
		/// <param name="to">user the call is intended for</param>
		/// <param name="from">where the call is from</param>
		/// <param name="originalTo">where the call is originally for</param>
		/// <param name="txIp">IP for media transmission</param>
		/// <param name="txPort">port for media transmission</param>
		/// <param name="caps">caps for media transmission</param>
        private void OnIncomingCall(string stackCallId, string dn, string to, string from, string originalTo, string txIp, int txPort, MediaCapsField caps)
        {
            long callId = base.callIdFactory.GenerateCallId();
			
			if (proxy.LogTimgingStat)
				log.Write(TraceLevel.Info, "OnIncomingCall: {0}", callId);

			//need to strip domain name from From field
			string domainName = null;
			string[] dns = dn.Split(Consts.FromFieldSeparator);
			if (dns == null || dns.Length < 2)
			{
				log.Write(TraceLevel.Warning, "Invalid device for incoming call: " + dn);
				domainName = "";
			}
			else
				domainName = dns[1];

			CallInfo cInfo = calls.AddCall(stackCallId, callId, to, from, CallDirection.Inbound, domainName);
			if (cInfo == null)
			{
				log.Write(TraceLevel.Error, "Could not create new incoming call {0} on device {1}", stackCallId, dn);
				return;
			}

			SendIncomingCall(callId, stackCallId, to, from, originalTo, caps!=null, from);
			if (caps != null)
			{
				SendGotCapabilities(callId, caps);
				SendMediaEstablished(callId, txIp, (uint)txPort, null, 0, 
					IMediaControl.Codecs.Unspecified, 0, 
					IMediaControl.Codecs.Unspecified, 0);
			}
        }

		/// <summary>
		/// It passes on Caps info to TM.
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="txIp">IP for media transmission</param>
		/// <param name="txPort">port for media transmission</param>
		/// <param name="caps">caps for media transmission</param>
		private void OnGotCapabilities(long cid, string txIp, int txPort, MediaCapsField caps)
		{
			CallInfo cInfo = calls.GetCall(cid);
			if(cInfo == null)
			{
				CallInfo ended = calls.RecentlyEndedCall(cid);
                if(ended != null)
				{
					log.Write(TraceLevel.Warning, "Received OnGotCapabilities message for non-existent call: " + cid);
					proxy.SendHangup(ended.StackCallId);
				}
				return;
			}

			SendGotCapabilities(cid, caps);
			SendMediaEstablished(cid, txIp, (uint)txPort, null, 0, 
				IMediaControl.Codecs.Unspecified, 0, 
				IMediaControl.Codecs.Unspecified, 0);
		}

		/// <summary>
		/// It removes the call from cache. If the call is still in pending state,
		/// it notifies TM about the CallSetupFailure.
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="stackCallId">stack call id for the call</param>
		/// <param name="cause">the reason for hanging up</param>
        private void OnHangup(long cid, string stackCallId, string cause)
        {
            CallInfo cInfo = calls.GetCall(cid);
			if (cInfo == null)
				cInfo = calls.GetCall(stackCallId);
            if(cInfo != null)
            {
				lock(cInfo.CallLock)
				{
					calls.RemoveCall(cInfo.CallId);

					if(cInfo.Direction == CallDirection.Outbound && 
						(cInfo.State == CallState.Init || cInfo.State == CallState.PendingOutbound))
					{
						base.SendCallSetupFailed(cInfo.CallId, ICallControl.EndReason.Ringout);
					}
					else
					{
						base.SendHangup(cInfo.CallId);
					}
				}
            }
        }

		/// <summary>
		/// It forwards the digits to TM.
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="digits">the digits to be forwarded</param>
		/// <param name="source">where the digits coming from</param>
        private void OnGotDigits(long cid, string digits, uint source)
        {
            CallInfo cInfo = calls.GetCall(cid);
            if(cInfo == null)
            {
				CallInfo ended = calls.RecentlyEndedCall(cid);
                if(ended != null)
                {
                    log.Write(TraceLevel.Warning, "Received GotDigits message for non-existent call: " + cid);
                    proxy.SendHangup(ended.StackCallId);
                }
                return;
            }

			lock(cInfo.CallLock)
			{
				log.Write(TraceLevel.Verbose, "Got digit(s) '{0}' in call: {1}", digits, cInfo.CallId);

				if((Source)source == Source.Remote)  // Ignore local digit events
				{
					base.SendGotDigits(cInfo.CallId, digits);
				}
			}
        }

		/// <summary>
		/// It responds to ReInvite request. Depending on different call state, it either
		/// sends stack its current media info (if it is already in hold state) or it notifies
		/// TM about the request.
		/// </summary>
		/// <param name="cid">identifies the call</param>
		/// <param name="txIp">IP for media transmission</param>
		/// <param name="txPort">port for media transmission</param>
		/// <param name="caps">caps for media transmission</param>
		/// <param name="mediaActive">false if it is generated by Hold request</param>
		/// <param name="isAnswer">true if it's answer for earlier reinvite</param>
		private void OnReInvite(long cid, string txIp, int txPort, MediaCapsField caps, bool mediaActive, bool isAnswer)
		{
			if (proxy.LogTimgingStat)
				log.Write(TraceLevel.Info, "OnReInvite: {0}", cid);

			CallInfo cInfo = calls.GetCall(cid);
			if(cInfo == null)
			{
				CallInfo ended = calls.RecentlyEndedCall(cid);
				if(ended != null)
				{
					log.Write(TraceLevel.Warning, "Received ReInvite message for non-existent call: " + cid);
					proxy.SendHangup(ended.StackCallId);
				}
				return;
			}

			IMediaControl.Codecs codec = IMediaControl.Codecs.Unspecified;
			uint fms = 0;
			bool diffMedia = false;


			lock(cInfo.CallLock)
			{
				if (cInfo.State == CallState.OnHold && !isAnswer)	//if it is already in onhold state
				{
					//simply answer back if it is a request, otherwise just ignore it
//					if (!isAnswer)
					{
						//send SetMedia so the stack can generate an offer
						proxy.SendSetMedia(cInfo.CallId, cInfo.StackCallId, cInfo.RxIp, cInfo.RxPort,
							cInfo.Codec, cInfo.Framesize, cInfo.MediaCaps);
					}
//					else
//					{
//						log.Write(TraceLevel.Info, "Received an answer for ReInvite that's on hold. Nothing more to do.");
//					}

				} //if (CallState==OnHold)
				else
				{
					if (isAnswer) //got the answer for the re-invite
					{
					} //cInfo.State = CallState.Active;
					else //if it is an re-invite, not answer to re-invite
						cInfo.State = CallState.ReInviting;

					//if there is only one codec specified, use it as negotiated codec
					if (caps != null && caps.Count == 1)
					{
						//take the first codec in caps and use that as the negotiated codec
						IEnumerator it = caps.GetEnumerator();
						it.MoveNext();

						codec = (IMediaControl.Codecs) ((DictionaryEntry) it.Current).Key;
						fms = caps.GetFramesizes(codec)[0];

						diffMedia = !(cInfo.TxIp == txIp && cInfo.TxPort == txPort && 
							cInfo.Codec == codec && cInfo.Framesize == fms);
					}					
					//always send up the change if mediaActive is false, which
					//means a hold is requested
					//otherwise send up the change only if there is difference
					if (isAnswer || !mediaActive || diffMedia) 
					{
						cInfo.TxIp = txIp;
						cInfo.TxPort = txPort;
						cInfo.Codec = codec;
						cInfo.Framesize = fms;

						if (!mediaActive || Consts.NullIp.Equals(cInfo.TxIp))
						{
							//						txIp = null;
							txPort = 0;
						}

						SendMediaChanged(cInfo.CallId, txIp, (uint)txPort, txIp, (uint)txPort,
							cInfo.Codec, cInfo.Framesize, cInfo.Codec, cInfo.Framesize);

						//					SendGotCapabilities(cid, caps);		
					}
					else	//generated by a reinvite without offer or re-invite without change
					{
						if (cInfo.State == CallState.ReInviting) //expects an answer
						{
							//send SetMedia so the stack can generate an offer
							proxy.SendSetMedia(cInfo.CallId, cInfo.StackCallId, cInfo.RxIp, cInfo.RxPort,
								cInfo.Codec, cInfo.Framesize, cInfo.MediaCaps);
							cInfo.State = CallState.Active;
						}
						else
						{
							log.Write(TraceLevel.Warning, "Invalid state for ReInvite: {0}.", cInfo.CallId);
						}
					}
				}//else of if (CallState==OnHold)
			}//lock
		}//OnReInvite

		/// <summary>
		/// It changes the device to be Stopped and resets its directory number to be empty.
		/// </summary>
		/// <param name="dn">the old directory number of the device</param>
		public void OnResetDirectoryNumber(string dn)
		{
			SipDeviceInfo di = LookupDeviceFromDN(dn);
			if (di != null)
			{
				log.Write(TraceLevel.Info, "Reset device with Directory Number: {0}", dn);
				di.Status = IConfig.Status.Enabled_Stopped;
				configUtility.UpdateDeviceStatus(di.Name, di.Type, di.Status);
				configUtility.SetDirectoryNumber(di.Name, di.Type, "");

				ccmDnrequestor.AddRequest(di);
			}
			else
			{
				log.Write(TraceLevel.Error, "Can't reset directory number. There is no device with Directory Number: {0}", dn);
			}
		}

        #endregion

		#region Callback directly from proxy
		/// <summary>
		/// Callback for requesting directory number. It simply adds a request to
		/// the DN request queue.
		/// </summary>
		/// <param name="di">the device for which to request</param>
		public void OnRequestDirectoryNumber(SipDeviceInfo di)
		{
			ccmDnrequestor.AddRequest(di);
		}

		#endregion

        #region Private helper methods

        /// <summary>Gets the set of all CTI devices in the AppServer database</summary>
        /// <returns>DeviceInfo Array</returns>
        private ArrayList GetSipDevicesFromDb()
        {
            ArrayList dbDevices = new ArrayList();

            ComponentInfo[] poolGroups = configUtility.GetComponents(IConfig.ComponentType.Cisco_SIP_DevicePool);
            if(poolGroups != null)
            {
                foreach(ComponentInfo cInfo in poolGroups)
                {
					Core.ConfigData.SipDeviceInfo[] devs = configUtility.GetSipDevices(cInfo);
                    if(devs == null) 
                        continue;
                    else
                        dbDevices.AddRange(devs);
                }
            }

            ComponentInfo[] routeGroups = configUtility.GetComponents(IConfig.ComponentType.SIP_Trunk);
            if(routeGroups != null)
            {
                foreach(ComponentInfo cInfo in routeGroups)
                {
                    Core.ConfigData.SipDeviceInfo[] devs = configUtility.GetSipDevices(cInfo);
					if(devs == null) 
						continue;
					else
					{
						//mark all trunk devices as enabled
						foreach (SipDeviceInfo di in devs)
							di.Status = IConfig.Status.Enabled_Running;
						dbDevices.AddRange(devs);
					}
                }
            }

            return dbDevices;
		}

		/// <summary>
		/// It retrieves free device from the device pool
		/// </summary>
		/// <param name="sp">the device pool where to retrieve free devices</param>
		/// <returns>a free device from the pool</returns>
        private Core.ConfigData.SipDeviceInfo GetFreeDevice(SipDevicePool sp)
        {
			string key;
            foreach(string deviceName in sp.DeviceNames)
            {
				key = deviceName/*WORKAROUND FOR CCM +"@"+sp.Domain*/;
				Core.ConfigData.SipDeviceInfo dInfo = (Core.ConfigData.SipDeviceInfo) devices[key];
				if (dInfo != null &&
					dInfo.Status == IConfig.Status.Enabled_Running)
				{
					return dInfo;
				}
            }
            return null;
        }

		/// <summary>
		/// It looks up a device from cache based on directory number
		/// </summary>
		/// <param name="dn">directory number to be looked up</param>
		/// <returns>the device with the directory number</returns>
		private SipDeviceInfo LookupDeviceFromDN(string dn)
		{
			SipDeviceInfo rc = null;
			foreach(SipDeviceInfo di in devices.Values)
			{
				if (dn.Equals(di.DirectoryNumber))
				{
					rc = di;
					break;
				}
			}

			return rc;
		}

		/// <summary>
		/// It retrieves a free device from a trunk.
		/// </summary>
		/// <param name="st">the trunk where the free device should come from</param>
		/// <returns>the free device</returns>
		private Core.ConfigData.SipDeviceInfo GetFreeDevice(SipTrunk st)
		{
			string key = st.ComponentInfo.name/*WORKAROUND FOR CCM + "@" + st.Domain*/;
			Core.ConfigData.SipDeviceInfo dInfo = (Core.ConfigData.SipDeviceInfo) devices[key];
			if (dInfo != null &&
				dInfo.Status == IConfig.Status.Enabled_Running)
			{
				return dInfo;
			}
			return null;
		}

		#endregion
    }
}
