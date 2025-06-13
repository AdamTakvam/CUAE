using System;
using System.Net;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Collections.Specialized;

using Metreos.Core;
using Metreos.Messaging;
using Metreos.Utilities;
using Metreos.Interfaces;
using Metreos.Core.ConfigData;
using Metreos.LoggingFramework;
using Metreos.ProviderFramework;
using Metreos.Messaging.MediaCaps;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.Core.ConfigData.CallRouteGroups;

using Package = Metreos.Interfaces.PackageDefinitions.JTapi;
using useType = Metreos.PackageGeneratorCore.PackageXml.useType;

namespace Metreos.CallControl.JTapi
{
	[ProviderDecl(Package.Globals.PACKAGE_NAME)]
	[PackageDecl(Package.Globals.NAMESPACE, Package.Globals.PACKAGE_DESCRIPTION)]
	public class JTapiProvider : CallControlProviderBase
	{
        #region Constants

        private abstract class Consts
        {
            public const int MediaTimeout       = 5000;
            public const int RingingTimeout     = 3000;

            public const int MorgueSize         = 100;

            public static string[] ServiceNames = 
            {
                "3.3",
                "4.0",
                "4.1",
                "4.2",
                "5.0",
                "5.1",
                "6.0"
            };

            // Local loopback ports on which the various JTapi service versions are listening.
            // Must correlate precisely with ServiceNames.
            public static int[] ServicePorts =
            {
                9120,    // 3.3
                9100,    // 4.0
                9110,    // 4.1
                9130,    // 4.2
                9140,    // 5.0
                9150,    // 5.1
                9160     // 6.0
            };

            public static string[] NonFatalErrors = 
            { 
                "CTIERR_DIGIT_GENERATION_CALLSTATE_CHANGED",
                "CTIERR_DIGIT_GENERATION_WRONG_CALL_STATE"
            };

            //public abstract class Actions
            //{
            //    public const string MakeCall    = ExtNamespace + "." + Suffixes.MakeCall;
            //    public const string AnswerCall  = ExtNamespace + "." + Suffixes.AnswerCall;
            //    public const string RejectCall  = ExtNamespace + "." + Suffixes.RejectCall;
            //    public const string Conference  = ExtNamespace + "." + Suffixes.Conference;
            //    public const string Redirect    = ExtNamespace + "." + Suffixes.Redirect;
            //    public const string BlindXfer   = ExtNamespace + "." + Suffixes.BlindXfer;
            //    public const string Hangup      = ExtNamespace + "." + Suffixes.Hangup;

            //    public abstract class Suffixes
            //    {
            //        public const string MakeCall    = "JTapiMakeCall";
            //        public const string AnswerCall  = "JTapiAnswerCall";
            //        public const string RejectCall  = "JTapiRejectCall";
            //        public const string Conference  = "JTapiConference";
            //        public const string Redirect    = "JTapiRedirect";
            //        public const string BlindXfer   = "JTapiBlindTransfer";
            //        public const string Hangup      = "JTapiHangup";
            //    }

            //    public abstract class Descriptions
            //    {
            //        public const string MakeCall    = "Places a call from a monitored device.";
            //        public const string AnswerCall  = "Answers the specified third-party call.";
            //        public const string RejectCall  = "Reject the specified third-party call.";
            //        public const string Conference  = "Conferences the specified third-party calls.";
            //        public const string Redirect    = "Redirects an unanswered third-party call to another line.";
            //        public const string BlindXfer   = "Transfers an established third-party call to another line.";
            //        public const string Hangup      = "Terminates the specified third-party call.";
            //    }
            //}

            //public abstract class Events
            //{
            //    public const string IncomingCall    = ExtNamespace + "." + Suffixes.IncomingCall;
            //    public const string CallInitiated   = ExtNamespace + "." + Suffixes.CallInitiated;
            //    public const string CallEstablished = ExtNamespace + "." + Suffixes.CallEstablished;
            //    public const string CallActive      = ExtNamespace + "." + Suffixes.CallActive;
            //    public const string CallInactive    = ExtNamespace + "." + Suffixes.CallInactive;
            //    public const string Hangup          = ExtNamespace + "." + Suffixes.Hangup;
            //    public const string GotDigits       = ExtNamespace + "." + Suffixes.GotDigits;

            //    public abstract class Suffixes
            //    {
            //        public const string IncomingCall    = "JTapiIncomingCall";
            //        public const string CallInitiated   = "JTapiCallInitiated";
            //        public const string CallEstablished = "JTapiCallEstablished";
            //        public const string CallActive      = "JTapiCallActive";
            //        public const string CallInactive    = "JTapiCallInactive";
            //        public const string Hangup          = "JTapiHangup";
            //        public const string GotDigits       = "JTapiGotDigits";
            //    }

            //    public abstract class Descriptions
            //    {
            //        public const string IncomingCall    = "Indicates that a call has been received on a monitored device";
            //        public const string CallInitiated   = "Indicates that a monitored device has gone off-hook";
            //        public const string CallEstablished = "Indicates that a monitored device has placed a call";
            //        public const string CallActive      = "Indicates that a call is active on a monitored device";
            //        public const string CallInactive    = "Indicates that a call is inactive (held) on a monitored device";
            //        public const string Hangup          = "Indicates that a call has been terminated on a monitored device";
            //        public const string GotDigits       = "Indicates that digits have been pressed by a monitored device";
            //    }
            //}

            //public abstract class Fields
            //{
            //    public const string InUse           = "InUse";
            //    public const string VolatileCallId  = "VolatileCallId";
            //    public const string DeviceName      = "DeviceName";
            //    public const string Digits          = "Digits";
            //    public const string Source          = "Source";

            //    public abstract class Descriptions
            //    {
            //        public const string InUse           = "If this field is true, another device is controlling the line. Otherwise, the call is simply on hold";
            //        public const string VolatileCallId  = "Call ID which will become invalidated by this action";
            //        public const string DeviceName      = "Name of device";
            //        public const string Digits          = "The digits pressed";
            //        public const string Source          = "Indicates whether the digit was pressed by the 'Local' or 'Remote' party";
            //    }
            //}

            public abstract class ConfigValueNames
            {
                public const string MaxCallsPerDevice   = "MaxCallsPerDevice";
                public const string UseLbrCodecs        = "UseLbrCodecs";
//                public const string UsingMoh            = "UsingMoh";
            }

            public abstract class DefaultValues
            {
                public const uint MaxCallsPerDevice = 1;
            }
        }

        public enum Source : uint
        {
            Unspecified = 0,
            Remote      = 1,
            Local       = 2
        }

        #endregion

        /// <summary>Table of JTAPI service connections (proxies)</summary>
        private readonly Hashtable proxies;

        /// <summary>Table of DeviceInfo objects</summary>
        private readonly DeviceTable devices;

        /// <summary>List of non-fatal error messages which can be received from the service</summary>
        private readonly StringCollection nonFatalErrors;

        /// <summary>Indicates whether or not to register devices with LBR codec support</summary>
        private bool useLbrCodecs = false;

        private bool startupComplete = false;

        #region Construction/Init/Startup/Refresh/Shutdown/Cleanup

        public JTapiProvider(IConfigUtility configUtility, ICallIdFactory callIdFactory)
            : base(typeof(JTapiProvider), Package.Globals.DISPLAY_NAME, IConfig.ComponentType.CTI_DevicePool,
            configUtility, callIdFactory)
        {
            this.devices = new DeviceTable(Consts.MorgueSize, configUtility, log);
            this.proxies = new Hashtable();

            this.nonFatalErrors = new StringCollection();
            this.nonFatalErrors.AddRange(Consts.NonFatalErrors);
        }

        protected override bool DeclareConfig(out ConfigEntry[] configItems, out Extension[] extensions)
        {
            base.messageCallbacks.Add(Package.Actions.JTapiAnswerCall.FULLNAME, 
                new HandleMessageDelegate(this.PropHandleAnswerCall));
            base.messageCallbacks.Add(Package.Actions.JTapiConference.FULLNAME, 
                new HandleMessageDelegate(this.PropHandleConference));
            base.messageCallbacks.Add(Package.Actions.JTapiRedirect.FULLNAME,
                new HandleMessageDelegate(this.PropHandleRedirect));
            base.messageCallbacks.Add(Package.Actions.JTapiBlindTransfer.FULLNAME, 
                new HandleMessageDelegate(this.PropHandleBlindTransfer));
            base.messageCallbacks.Add(Package.Actions.JTapiHangup.FULLNAME,
                new HandleMessageDelegate(this.PropHandleHangupCall));
            base.messageCallbacks.Add(Package.Actions.JTapiRejectCall.FULLNAME,
                new HandleMessageDelegate(this.PropHandleRejectCall));
            base.messageCallbacks.Add(Package.Actions.JTapiMakeCall.FULLNAME,
                new HandleMessageDelegate(this.PropHandleMakeCall));

            configItems = new ConfigEntry[2];

            configItems[0] = new ConfigEntry(Consts.ConfigValueNames.MaxCallsPerDevice, "Max Calls per Device", 
                Consts.DefaultValues.MaxCallsPerDevice, 
                "Maximum number of calls allowed on any first-party CTI Port device (as configured in CallManager)", 
                1, 200, true);
            configItems[1] = new ConfigEntry(Consts.ConfigValueNames.UseLbrCodecs, "Advertise Low-bitrate Codecs",
                false, "Indicates whether devices should be registered with G.723.1 and G.729a support.", 
                IConfig.StandardFormat.Bool, true);
//            configItems[2] = new ConfigEntry(Consts.ConfigValueNames.UsingMoh, "Using Music-On-Hold",
//                true, "Indicates whether CallManager is configured with a music-on-hold source.", 
//                IConfig.StandardFormat.Bool, true);

            extensions = null;

            return true;
        }

        protected override void OnStartup()
		{
            // Create the JTapi service proxies
            for(int i=0; i<Consts.ServiceNames.Length; i++)
            {
                JTapiProxy proxy = CreateJTapiProxy(Consts.ServiceNames[i]);
                this.proxies[Consts.ServiceNames[i]] = proxy;
            }

			startupComplete = true;   // It's a white lie  ;)

            // Build device list
            RefreshConfiguration();

            // Start the proxies
            for(int i=0; i<Consts.ServiceNames.Length; i++)
            {
                JTapiProxy proxy = this.proxies[Consts.ServiceNames[i]] as JTapiProxy;
                proxy.Startup(new IPEndPoint(IPAddress.Loopback, Consts.ServicePorts[i]));
            }

            // Set the log Level explicitly after startup of services
            for(int i=0; i<Consts.ServiceNames.Length; i++)
            {
                JTapiProxy proxy = this.proxies[Consts.ServiceNames[i]] as JTapiProxy;
                proxy.SendSetLogLevel((int) GetLogLevel());
            }

            // Call Control namespace registration
            base.RegisterNamespace();
            base.RegisterSecondaryProtocolType(IConfig.ComponentType.CTI_RoutePoint);

            // Proprietary extension namespace registration
            base.RegisterExtensionNamespace(Package.Globals.NAMESPACE);
        }

        private MediaCapsField OnGetCodecs()
        {
            MediaCapsField mediaCaps = new MediaCapsField();
            mediaCaps.Add(IMediaControl.Codecs.G711u, 10, 20, 30);
            mediaCaps.Add(IMediaControl.Codecs.G711a, 10, 20, 30);

            if(useLbrCodecs)
            {
                mediaCaps.Add(IMediaControl.Codecs.G729, 10, 20, 30);
            }
            return mediaCaps;
        }

        private DeviceInfo[] OnGetDevices(string name)
        {
            Assertion.Check(devices != null, "Device table is null in JTAPI provider");
            return devices.GetEnabledDevices(name);
        }
  
        protected override void RefreshConfiguration()
        {
            if(!startupComplete)
                return;

            // Set the log Level
            for(int i=0; i<Consts.ServiceNames.Length; i++)
            {
                JTapiProxy proxy = this.proxies[Consts.ServiceNames[i]] as JTapiProxy;
                proxy.SendSetLogLevel((int) GetLogLevel());
            }

            // Update max number of calls per device
            devices.MaxCallsPerDevice = Convert.ToUInt32(base.GetConfigValue(Consts.ConfigValueNames.MaxCallsPerDevice));

            // Update LBR codec support flag
            this.useLbrCodecs = Convert.ToBoolean(base.GetConfigValue(Consts.ConfigValueNames.UseLbrCodecs));

            // See if any new devices have been added (first and/or third-party).
            ArrayList configDevices = GetCtiDevicesFromDb();
            if(configDevices == null)
                configDevices = new ArrayList();

            StringCollection configDevNames = new StringCollection();

            foreach(DeviceInfo dInfo in configDevices)
            {
                if(devices.Contains(dInfo.Name) == false)
                {
                    JTapiDeviceInfo jdInfo = devices.AddDevice(dInfo, GetProxy(dInfo.ClusterVersion));

                    if(jdInfo.Proxy.SendRegister(dInfo))
                    {
                        log.Write(TraceLevel.Info, "Registering new device '{0}' with JTAPI service v{1}",
                            dInfo.Name, dInfo.ClusterVersion);
                    }
                }

                configDevNames.Add(dInfo.Name);
            }

            // See if any devices have been removed
            foreach(JTapiDeviceInfo jdInfo in devices)
            {
                if(configDevNames.Contains(jdInfo.DeviceInfo.Name) == false)
                {
                    devices.RemoveDevice(jdInfo.DeviceInfo.Name);
                    
                    if(jdInfo.Proxy.SendUnregister(jdInfo.DeviceInfo))
                        log.Write(TraceLevel.Info, "Unregistering device: " + jdInfo.DeviceInfo.Name);
                }
            }
        }

        private JTapiProxy CreateJTapiProxy(string name)
        {
            JTapiProxy proxy = new JTapiProxy(name, log);
            proxy.onServiceGone = new NameDelegate(StopAllDevices);
            proxy.onGetCodecs = new GetCodecsDelegate(OnGetCodecs);
            proxy.onGetDevices = new GetDevicesDelegate(OnGetDevices);

            proxy.onCallEstablished = new OnCallEstablishedDelegate(OnCallEstablished);
            proxy.onAnswered = new OnCallEstablishedDelegate(OnAnswered);
            proxy.onError = new OnErrorDelegate(OnError);
            proxy.onMakeCallAck = new CallNoticeDelegate(OnMakeCallAck);
            proxy.onHangup = new OnHangupDelegate(OnHangup);
            proxy.onIncomingCall = new OnIncomingCallDelegate(OnIncomingCall);
            proxy.onRinging = new CallNoticeDelegate(OnRinging);
            proxy.onMediaEstablished = new OnMediaEstablishedDelegate(OnMediaEstablished);
            proxy.onStatusUpdate = new OnStatusUpdateDelegate(OnStatusUpdate);
            proxy.onCallInitiated = new OnCallInitiatedDelegate(PropOnCallInitiated);
            proxy.onCallInactive = new OnCallInactiveDelegate(PropOnCallInactive);
            proxy.onGotDigits = new OnGotDigitsDelegate(OnGotDigits);

            return proxy;
        }

        private JTapiProxy GetProxy(string version)
        {
            return this.proxies[version] as JTapiProxy;
        }

        private void StopAllDevices(string serviceVersion)
        {
            try
            {
                // Change the status of all device from Enabled_Running to Enabled_Stopped
                foreach(DeviceInfo dInfo in devices.GetEnabledDevices(serviceVersion))
                {
                    devices.SetStatus(dInfo.Name, IConfig.Status.Enabled_Stopped);
                }
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Stopping devices: " + e);
            }
        }

        protected override void OnShutdown()
        {
            try
            {
                StopAllDevices(null);

                foreach(JTapiProxy proxy in proxies.Values)
                {
                    proxy.Shutdown();
                    proxy.Dispose();
                }

                devices.Clear();

                startupComplete = false;
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, "Shutdown exception: " + e);
            }
        }

        public override void Cleanup()
        {
            devices.Clear();
            base.Cleanup();
        }

        #endregion

        #region Proprietary action handlers

        [Action(Package.Actions.JTapiAnswerCall.FULLNAME, false, Package.Actions.JTapiAnswerCall.DISPLAY, Package.Actions.JTapiAnswerCall.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JTapiAnswerCall.Params.CallId.NAME, Package.Actions.JTapiAnswerCall.Params.CallId.DISPLAY, typeof(long), useType.required, false, Package.Actions.JTapiAnswerCall.Params.CallId.DESCRIPTION, Package.Actions.JTapiAnswerCall.Params.CallId.DEFAULT)]
        [ReturnValue()]
        private void PropHandleAnswerCall(ActionBase actionBase)
        {
            if (actionBase.InnerMessage.Contains(Package.Actions.JTapiAnswerCall.Params.CallId.NAME) == false)
            {
                log.Write(TraceLevel.Error, "Received third-party answer action with no call ID specified");
                actionBase.SendResponse(false);
                return;
            }

            long callId = Convert.ToInt64(actionBase.InnerMessage[Package.Actions.JTapiAnswerCall.Params.CallId.NAME]);

            bool success = true;

            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to answer a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Error, "Failed to answer third-party call. Bad call ID: " + callId);

                success = false;
            }
            else if(cInfo.Error)
                success = false;
            else if(cInfo.State == CallState.Init)
                cInfo.State = CallState.Answered;
            else if(cInfo.State == CallState.Ringing)
            {
                cInfo.State = CallState.Answered;
                cInfo.Proxy.SendAnswerCall(cInfo.StackCallId);
            }
            else
            {
                log.Write(TraceLevel.Error, "Cannot answer call {0}, invalid state: {1}",
                    callId, cInfo.State.ToString());
            }

            actionBase.SendResponse(success);
        }

        [Action(Package.Actions.JTapiRejectCall.FULLNAME, false, Package.Actions.JTapiRejectCall.DISPLAY, Package.Actions.JTapiRejectCall.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JTapiRejectCall.Params.CallId.NAME, Package.Actions.JTapiRejectCall.Params.CallId.DISPLAY, typeof(long), useType.required, false, Package.Actions.JTapiRejectCall.Params.CallId.DESCRIPTION, Package.Actions.JTapiRejectCall.Params.CallId.DEFAULT)]
        [ReturnValue()]
        private void PropHandleRejectCall(ActionBase actionBase)
        {
            if (actionBase.InnerMessage.Contains(Package.Actions.JTapiRejectCall.Params.CallId.NAME) == false)
            {
                log.Write(TraceLevel.Error, "Received third-party reject action with no call ID specified");
                actionBase.SendResponse(false);
                return;
            }

            long callId = Convert.ToInt64(actionBase.InnerMessage[Package.Actions.JTapiRejectCall.Params.CallId.NAME]);

            bool success = HandleTerminateCall(callId);

            actionBase.SendResponse(success);
        }

        [Action(Package.Actions.JTapiHangup.FULLNAME, false, Package.Actions.JTapiHangup.DISPLAY, Package.Actions.JTapiHangup.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JTapiHangup.Params.CallId.NAME, Package.Actions.JTapiHangup.Params.CallId.DISPLAY, typeof(long), useType.required, false, Package.Actions.JTapiHangup.Params.CallId.DESCRIPTION, Package.Actions.JTapiHangup.Params.CallId.DEFAULT)]
        [ReturnValue()]
        private void PropHandleHangupCall(ActionBase actionBase)
        {
            if (actionBase.InnerMessage.Contains(Package.Actions.JTapiHangup.Params.CallId.NAME) == false)
            {
                log.Write(TraceLevel.Error, "Received third-party hangup action with no call ID specified");
                actionBase.SendResponse(false);
                return;
            }

            long callId = Convert.ToInt64(actionBase.InnerMessage[Package.Actions.JTapiHangup.Params.CallId.NAME]);

            bool success = HandleTerminateCall(callId);

            actionBase.SendResponse(success);
        }

        [Action(Package.Actions.JTapiMakeCall.FULLNAME, false, Package.Actions.JTapiMakeCall.DISPLAY, Package.Actions.JTapiMakeCall.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JTapiMakeCall.Params.DeviceName.NAME, Package.Actions.JTapiMakeCall.Params.DeviceName.DISPLAY, typeof(string), useType.required, false, Package.Actions.JTapiMakeCall.Params.DeviceName.DESCRIPTION, Package.Actions.JTapiMakeCall.Params.DeviceName.DEFAULT)]
        [ActionParam(Package.Actions.JTapiMakeCall.Params.From.NAME, Package.Actions.JTapiMakeCall.Params.From.DISPLAY, typeof(string), useType.optional, false, Package.Actions.JTapiMakeCall.Params.From.DESCRIPTION, Package.Actions.JTapiMakeCall.Params.From.DEFAULT)]
        [ActionParam(Package.Actions.JTapiMakeCall.Params.To.NAME, Package.Actions.JTapiMakeCall.Params.To.DISPLAY, typeof(string), useType.required, false, Package.Actions.JTapiMakeCall.Params.To.DESCRIPTION, Package.Actions.JTapiMakeCall.Params.To.DEFAULT)]
        [ResultData(Package.Actions.JTapiMakeCall.Results.CallId.NAME, Package.Actions.JTapiMakeCall.Results.CallId.DISPLAY, typeof(string), Package.Actions.JTapiMakeCall.Results.CallId.DESCRIPTION)]
        [ReturnValue()]
        private void PropHandleMakeCall(ActionBase actionBase)
        {
            string deviceName = Convert.ToString(actionBase.InnerMessage[Package.Actions.JTapiMakeCall.Params.DeviceName.NAME]);
            if(deviceName == null || deviceName == String.Empty)
            {
                log.Write(TraceLevel.Error, "Received third-party MakeCall action with no call ID specified");
                actionBase.SendResponse(false);
                return;
            }

            string to = Convert.ToString(actionBase.InnerMessage[Package.Actions.JTapiMakeCall.Params.To.NAME]);
            if(to == null || to == String.Empty)
            {
                log.Write(TraceLevel.Error, "Received third-party MakeCall action with no destination specified");
                actionBase.SendResponse(false);
                return;
            }

            string from = Convert.ToString(actionBase.InnerMessage[Package.Actions.JTapiMakeCall.Params.From.NAME]);

            JTapiDeviceInfo jdInfo = devices[deviceName];
            if(jdInfo == null)
            {
                log.Write(TraceLevel.Error, "Failed to place third-party call. Device is not monitored: " + deviceName);
                actionBase.SendResponse(false);
                return;
            }

            if(jdInfo.ThirdParty == false)
            {
                log.Write(TraceLevel.Error, "Failed to place third-party call. Device is first-party: " + deviceName);
                actionBase.SendResponse(false);
                return;
            }

            long callId = callIdFactory.GenerateCallId();
			if (devices.AddCall_3P(actionBase.RoutingGuid, callId.ToString(), callId, deviceName, 
				to, deviceName, CallDirection.Outbound) == null)
			{
				log.Write(TraceLevel.Error, "Failed to place third-party call. Device is unknown: " + deviceName);
				actionBase.SendResponse(false);
				return;
			}

            bool success = jdInfo.Proxy.SendMakeCall(callId.ToString(), to, from, deviceName, IConfig.DeviceType.CtiMonitored, null, 0);
            if(success == false)
            {
                devices.RemoveCall(callId);
				actionBase.SendResponse(false);
				return;
            }

            ArrayList fields = new ArrayList();
            fields.Add(new Field(Package.Actions.JTapiMakeCall.Results.CallId.NAME, callId));

            actionBase.SendResponse(success, fields);
        }

        [Action(Package.Actions.JTapiConference.FULLNAME, false, Package.Actions.JTapiConference.DISPLAY, Package.Actions.JTapiConference.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JTapiConference.Params.CallId.NAME, Package.Actions.JTapiConference.Params.CallId.DISPLAY, typeof(long), useType.required, false, Package.Actions.JTapiConference.Params.CallId.DESCRIPTION, Package.Actions.JTapiConference.Params.CallId.DEFAULT)]
        [ActionParam(Package.Actions.JTapiConference.Params.VolatileCallId.NAME, Package.Actions.JTapiConference.Params.VolatileCallId.DISPLAY, typeof(long), useType.required, false, Package.Actions.JTapiConference.Params.VolatileCallId.DESCRIPTION, Package.Actions.JTapiConference.Params.VolatileCallId.DEFAULT)]
        [ReturnValue()]
        private void PropHandleConference(ActionBase actionBase)
        {
            if ((actionBase.InnerMessage.Contains(Package.Actions.JTapiConference.Params.CallId.NAME) == false) ||
                (actionBase.InnerMessage.Contains(Package.Actions.JTapiConference.Params.VolatileCallId.NAME) == false))
            {
                log.Write(TraceLevel.Error, "Received third-party conference action with no call IDs specified");
                actionBase.SendResponse(false);
                return;
            }

            long callId = Convert.ToInt64(actionBase.InnerMessage[Package.Actions.JTapiConference.Params.CallId.NAME]);
            long volCallId = Convert.ToInt64(actionBase.InnerMessage[Package.Actions.JTapiConference.Params.VolatileCallId.NAME]);

            bool success = true;

            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to conference a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Error, "Failed to conference. Call ID is invalid: " + callId);

                success = false;
            }

            CallInfo volCInfo = devices.GetCall(volCallId);
            if(volCInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to conference a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Error, "Failed to conference. Call ID is invalid: " + volCallId);

                success = false;
            }

            if(success)
            {
                success = cInfo.Proxy.SendConference(cInfo.StackCallId, volCInfo.StackCallId);
                if(success)
                {
                    // Volatile Call ID is good as dead. Let's not send the Hangup event up
                    devices.RemoveCall(volCallId);
                }
            }

            actionBase.SendResponse(success);
        }

        [Action(Package.Actions.JTapiRedirect.FULLNAME, false, Package.Actions.JTapiRedirect.DISPLAY, Package.Actions.JTapiRedirect.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JTapiRedirect.Params.CallId.NAME, Package.Actions.JTapiRedirect.Params.CallId.DISPLAY, typeof(long), useType.required, false, Package.Actions.JTapiRedirect.Params.CallId.DESCRIPTION, Package.Actions.JTapiRedirect.Params.CallId.DEFAULT)]
        [ActionParam(Package.Actions.JTapiRedirect.Params.To.NAME, Package.Actions.JTapiRedirect.Params.To.DISPLAY, typeof(string), useType.required, false, Package.Actions.JTapiRedirect.Params.To.DESCRIPTION, Package.Actions.JTapiRedirect.Params.To.DEFAULT)]
        [ReturnValue()]
        private void PropHandleRedirect(ActionBase actionBase)
        {
            bool success = true;

            long callId = Convert.ToInt64(actionBase.InnerMessage[Package.Actions.JTapiRedirect.Params.CallId.NAME]);
            string to = Convert.ToString(actionBase.InnerMessage[Package.Actions.JTapiRedirect.Params.To.NAME]);

            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to redirect a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Error, "Failed to redirect call. Call ID is invalid: " + callId);

                success = false;
            }

            if(success)
                success = cInfo.Proxy.SendRedirect(cInfo.StackCallId, to);

            actionBase.SendResponse(success);
        }

        [Action(Package.Actions.JTapiBlindTransfer.FULLNAME, false, Package.Actions.JTapiBlindTransfer.DISPLAY, Package.Actions.JTapiBlindTransfer.DESCRIPTION, false)]
        [ActionParam(Package.Actions.JTapiBlindTransfer.Params.CallId.NAME, Package.Actions.JTapiBlindTransfer.Params.CallId.DISPLAY, typeof(long), useType.required, false, Package.Actions.JTapiBlindTransfer.Params.CallId.DESCRIPTION, Package.Actions.JTapiBlindTransfer.Params.CallId.DEFAULT)]
        [ActionParam(Package.Actions.JTapiBlindTransfer.Params.To.NAME, Package.Actions.JTapiBlindTransfer.Params.To.DISPLAY, typeof(string), useType.required, false, Package.Actions.JTapiBlindTransfer.Params.To.DESCRIPTION, Package.Actions.JTapiBlindTransfer.Params.To.DEFAULT)]
        [ReturnValue()]
        private void PropHandleBlindTransfer(ActionBase actionBase)
        {
            bool success = true; 

            long callId = Convert.ToInt64(actionBase.InnerMessage[Package.Actions.JTapiBlindTransfer.Params.CallId.NAME]);
            string to = Convert.ToString(actionBase.InnerMessage[Package.Actions.JTapiBlindTransfer.Params.To.NAME]);

            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to transfer a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Error, "Failed to transfer call. Call ID is invalid: " + callId);

                success = false;
            }

            if(success)
                success = cInfo.Proxy.SendBlindTransfer(cInfo.StackCallId, to);

            actionBase.SendResponse(success);
        }

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

        #region Proprietary event senders
        [Event(Package.Events.JTapiIncomingCall.FULLNAME, true, null, Package.Events.JTapiIncomingCall.DISPLAY, Package.Events.JTapiIncomingCall.DESCRIPTION)]
        [EventParam(Package.Events.JTapiIncomingCall.Params.CallId.NAME, Package.Events.JTapiIncomingCall.Params.CallId.DISPLAY, typeof(long), true, Package.Events.JTapiIncomingCall.Params.CallId.DESCRIPTION)]
        [EventParam(Package.Events.JTapiIncomingCall.Params.DeviceName.NAME, Package.Events.JTapiIncomingCall.Params.DeviceName.DISPLAY, typeof(string), true, Package.Events.JTapiIncomingCall.Params.DeviceName.DESCRIPTION)]
        [EventParam(Package.Events.JTapiIncomingCall.Params.To.NAME, Package.Events.JTapiIncomingCall.Params.To.DISPLAY, typeof(string), true, Package.Events.JTapiIncomingCall.Params.To.DESCRIPTION)]
        [EventParam(Package.Events.JTapiIncomingCall.Params.From.NAME, Package.Events.JTapiIncomingCall.Params.From.DISPLAY, typeof(string), true, Package.Events.JTapiIncomingCall.Params.From.DESCRIPTION)]
        [EventParam(Package.Events.JTapiIncomingCall.Params.OriginalTo.NAME, Package.Events.JTapiIncomingCall.Params.OriginalTo.DISPLAY, typeof(string), true, Package.Events.JTapiIncomingCall.Params.OriginalTo.DESCRIPTION)]
        [EventParam(Package.Events.JTapiIncomingCall.Params.StackToken.NAME, Package.Events.JTapiIncomingCall.Params.StackToken.DISPLAY, typeof(string), true, Package.Events.JTapiIncomingCall.Params.StackToken.DESCRIPTION)]
        private void PropOnIncomingCall(long callId, string stackCallId, string deviceName, string to, string from, string originalTo)
        {
            CallInfo cInfo = devices.GetCall(callId);
            Assertion.Check(cInfo != null, "Could not retrieve newly created incoming call: " + callId);

            // Send the event
            cInfo.RoutingGuid = System.Guid.NewGuid().ToString();
            EventMessage eMsg = base.messageUtility.CreateEventMessage(Package.Events.JTapiIncomingCall.FULLNAME, 
                EventMessage.EventType.Triggering, cInfo.RoutingGuid);
            eMsg.AddField(Package.Events.JTapiIncomingCall.Params.CallId.NAME, callId);
            eMsg.AddField(Package.Events.JTapiIncomingCall.Params.StackToken.NAME, stackCallId);
            eMsg.AddField(Package.Events.JTapiIncomingCall.Params.DeviceName.NAME, deviceName);
            eMsg.AddField(Package.Events.JTapiIncomingCall.Params.To.NAME, to);
            eMsg.AddField(Package.Events.JTapiIncomingCall.Params.OriginalTo.NAME, originalTo);
            eMsg.AddField(Package.Events.JTapiIncomingCall.Params.From.NAME, from);

            log.Write(TraceLevel.Info, "Sending third-party IncomingCall for: " + callId);
            
            base.palWriter.PostMessage(eMsg);
        }

        [Event(Package.Events.JTapiCallInitiated.FULLNAME, true, null, Package.Events.JTapiCallInitiated.DISPLAY, Package.Events.JTapiCallInitiated.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInitiated.Params.CallId.NAME, Package.Events.JTapiCallInitiated.Params.CallId.DISPLAY, typeof(long), true, Package.Events.JTapiCallInitiated.Params.CallId.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInitiated.Params.DeviceName.NAME, Package.Events.JTapiCallInitiated.Params.DeviceName.DISPLAY, typeof(string), true, Package.Events.JTapiCallInitiated.Params.DeviceName.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInitiated.Params.From.NAME, Package.Events.JTapiCallInitiated.Params.From.DISPLAY, typeof(string), true, Package.Events.JTapiCallInitiated.Params.From.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInitiated.Params.StackToken.NAME, Package.Events.JTapiCallInitiated.Params.StackToken.DISPLAY, typeof(string), true, Package.Events.JTapiCallInitiated.Params.StackToken.DESCRIPTION)]
        private void PropOnCallInitiated(string stackCallId, string deviceName, string from)
        {
            // Sanity check first
            if(devices.Contains(deviceName) == false || devices.IsThirdParty(deviceName) == false)
                return;

            // This event indicates that the phone has gone off-hook
            // What we have to determine is whether or not we caused this or a human user did
            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo == null)
            {
                // Human user case
                long callId = base.callIdFactory.GenerateCallId();

				cInfo = devices.AddCall_3P(System.Guid.NewGuid().ToString(), stackCallId, callId, 
					deviceName, String.Empty, from, CallDirection.Outbound);

				if (cInfo == null)
				{
					log.Write(TraceLevel.Error, "Could not make new third-party call for {0}", stackCallId);
					return;
				}
            }

            // Send the event
            EventMessage eMsg = base.messageUtility.CreateEventMessage(Package.Events.JTapiCallInitiated.FULLNAME, 
                EventMessage.EventType.Triggering, cInfo.RoutingGuid);
            eMsg.AddField(Package.Events.JTapiCallInitiated.Params.CallId.NAME, cInfo.CallId);
            eMsg.AddField(Package.Events.JTapiCallInitiated.Params.StackToken.NAME, stackCallId);
            eMsg.AddField(Package.Events.JTapiCallInitiated.Params.DeviceName.NAME, deviceName);
            eMsg.AddField(Package.Events.JTapiCallInitiated.Params.From.NAME, from);

            log.Write(TraceLevel.Info, "Sending third-party CallInitiated for: " + cInfo.CallId);
                
            base.palWriter.PostMessage(eMsg);
        }

        [Event(Package.Events.JTapiCallEstablished.FULLNAME, true, null, Package.Events.JTapiCallEstablished.DISPLAY, Package.Events.JTapiCallEstablished.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallEstablished.Params.CallId.NAME, Package.Events.JTapiCallEstablished.Params.CallId.DISPLAY, typeof(long), true, Package.Events.JTapiCallEstablished.Params.CallId.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallEstablished.Params.DeviceName.NAME, Package.Events.JTapiCallEstablished.Params.DeviceName.DISPLAY, typeof(string), true, Package.Events.JTapiCallEstablished.Params.DeviceName.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallEstablished.Params.From.NAME, Package.Events.JTapiCallEstablished.Params.From.DISPLAY, typeof(string), true, Package.Events.JTapiCallEstablished.Params.From.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallEstablished.Params.To.NAME, Package.Events.JTapiCallEstablished.Params.To.DISPLAY, typeof(string), true, Package.Events.JTapiCallEstablished.Params.To.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallEstablished.Params.StackToken.NAME, Package.Events.JTapiCallEstablished.Params.StackToken.DISPLAY, typeof(string), true, Package.Events.JTapiCallEstablished.Params.StackToken.DESCRIPTION)]
        private void PropOnCallEstablished(CallInfo cInfo, string deviceName)
        {
            // Sanity check first
            if(devices.Contains(deviceName) == false || devices.IsThirdParty(deviceName) == false)
                return;

            // Send the event
            EventMessage eMsg = base.messageUtility.CreateEventMessage(Package.Events.JTapiCallEstablished.FULLNAME,
                EventMessage.EventType.Triggering, cInfo.RoutingGuid);
            eMsg.AddField(Package.Events.JTapiCallEstablished.Params.CallId.NAME, cInfo.CallId);
            eMsg.AddField(Package.Events.JTapiCallEstablished.Params.StackToken.NAME, cInfo.StackCallId);
            eMsg.AddField(Package.Events.JTapiCallEstablished.Params.DeviceName.NAME, deviceName);
            eMsg.AddField(Package.Events.JTapiCallEstablished.Params.To.NAME, cInfo.To);
            eMsg.AddField(Package.Events.JTapiCallEstablished.Params.From.NAME, cInfo.From);

            log.Write(TraceLevel.Info, "Sending third-party CallInitiated for: " + cInfo.CallId);

            base.palWriter.PostMessage(eMsg);
        }

        [Event(Package.Events.JTapiCallActive.FULLNAME, false, null, Package.Events.JTapiCallActive.DISPLAY, Package.Events.JTapiCallActive.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallActive.Params.CallId.NAME, Package.Events.JTapiCallActive.Params.CallId.DISPLAY, typeof(long), true, Package.Events.JTapiCallActive.Params.CallId.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallActive.Params.DeviceName.NAME, Package.Events.JTapiCallActive.Params.DeviceName.DISPLAY, typeof(string), true, Package.Events.JTapiCallActive.Params.DeviceName.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallActive.Params.To.NAME, Package.Events.JTapiCallActive.Params.To.DISPLAY, typeof(string), true, Package.Events.JTapiCallActive.Params.To.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallActive.Params.StackToken.NAME, Package.Events.JTapiCallActive.Params.StackToken.DISPLAY, typeof(string), true, Package.Events.JTapiCallActive.Params.StackToken.DESCRIPTION)]
        private void PropOnCallActive(CallInfo cInfo, string deviceName)
        {
            cInfo.State = CallState.Active;

            EventMessage eMsg = base.messageUtility.CreateEventMessage(Package.Events.JTapiCallActive.FULLNAME, 
                EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            eMsg.AddField(Package.Events.JTapiCallActive.Params.CallId.NAME, cInfo.CallId);
            eMsg.AddField(Package.Events.JTapiCallActive.Params.StackToken.NAME, cInfo.StackCallId);
            eMsg.AddField(Package.Events.JTapiCallActive.Params.DeviceName.NAME, deviceName);
            eMsg.AddField(Package.Events.JTapiCallActive.Params.To.NAME, cInfo.To);
            
            log.Write(TraceLevel.Info, "Sending CallActive for call: " + cInfo.CallId);

            base.palWriter.PostMessage(eMsg);
        }

        [Event(Package.Events.JTapiCallInactive.FULLNAME, false, null, Package.Events.JTapiCallInactive.DISPLAY, Package.Events.JTapiCallInactive.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInactive.Params.CallId.NAME, Package.Events.JTapiCallInactive.Params.CallId.DISPLAY, typeof(long), true, Package.Events.JTapiCallInactive.Params.CallId.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInactive.Params.DeviceName.NAME, Package.Events.JTapiCallInactive.Params.DeviceName.DISPLAY, typeof(string), true, Package.Events.JTapiCallInactive.Params.DeviceName.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInactive.Params.InUse.NAME, Package.Events.JTapiCallInactive.Params.InUse.DISPLAY, typeof(bool), true, Package.Events.JTapiCallInactive.Params.InUse.DESCRIPTION)]
        [EventParam(Package.Events.JTapiCallInactive.Params.StackToken.NAME, Package.Events.JTapiCallInactive.Params.StackToken.DISPLAY, typeof(string), true, Package.Events.JTapiCallInactive.Params.StackToken.DESCRIPTION)]
        private void PropOnCallInactive(string deviceName, string stackCallId, bool inUse)
        {
            // Make sure this is a third-party device
            if(devices.IsThirdParty(deviceName) == false)
            {
                // Silently drop it
                return;
            }

            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo == null && devices.IsRecentlyEnded(stackCallId) == false)
            {
                // Assume that a new outbound has been placed from another device which is 
                //  sharing a line with the one in question. Thus a new InUse call has spontaneously
                //  sprung into existence. Create this new call and construct a CallInitiated message
                //  to introduce it to the app, then send the InUse as normal. Inbound calls in this 
                //  scenario will not encounter this condition. (Yeah, I know, just go with it)
                long callId = base.callIdFactory.GenerateCallId();

                cInfo = devices.AddCall_3P(System.Guid.NewGuid().ToString(), stackCallId, callId, 
                    deviceName, String.Empty, String.Empty, CallDirection.Outbound);

                if (cInfo == null)
                {
                    log.Write(TraceLevel.Error, "Could not make new third-party call for {0}", stackCallId);
                    return;
                }

                PropOnCallInitiated(stackCallId, deviceName, String.Empty);
            }
            else if(cInfo == null)
            {
                // The call ended recently, silently drop this message
                return;
            }

            if(devices.Contains(deviceName) == false)
            {
                log.Write(TraceLevel.Error, "Received {0} message for unknown device: {1}",
                    inUse ? JTapiProxy.MsgType.CallInUse.ToString() : JTapiProxy.MsgType.CallHeld.ToString(),
                    deviceName);
                return;
            }

            if(inUse)
                cInfo.State = CallState.InUse;
            else
                cInfo.State = CallState.Held;

            EventMessage msg = base.messageUtility.CreateEventMessage(Package.Events.JTapiCallInactive.FULLNAME, 
                EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            msg.AddField(Package.Events.JTapiCallInactive.Params.CallId.NAME, cInfo.CallId);
            msg.AddField(Package.Events.JTapiCallInactive.Params.DeviceName.NAME, deviceName);
            msg.AddField(Package.Events.JTapiCallInactive.Params.StackToken.NAME, cInfo.StackCallId);
            msg.AddField(Package.Events.JTapiCallInactive.Params.InUse.NAME, inUse);
        
            log.Write(TraceLevel.Info, "Sending CallInactive (InUse = {0}) for call: {1}",
                inUse.ToString(), cInfo.CallId);

            base.palWriter.PostMessage(msg);
        }

        [Event(Package.Events.JTapiHangup.FULLNAME, false, null, Package.Events.JTapiHangup.DISPLAY, Package.Events.JTapiHangup.DESCRIPTION)]
        [EventParam(Package.Events.JTapiHangup.Params.CallId.NAME, Package.Events.JTapiHangup.Params.CallId.DISPLAY, typeof(long), true, Package.Events.JTapiHangup.Params.CallId.DESCRIPTION)]
        [EventParam(Package.Events.JTapiHangup.Params.DeviceName.NAME, Package.Events.JTapiHangup.Params.DeviceName.DISPLAY, typeof(string), true, Package.Events.JTapiHangup.Params.DeviceName.DESCRIPTION)]
        [EventParam(Package.Events.JTapiHangup.Params.Cause.NAME, Package.Events.JTapiHangup.Params.Cause.DISPLAY, typeof(string), true, Package.Events.JTapiHangup.Params.Cause.DESCRIPTION)]
        [EventParam(Package.Events.JTapiHangup.Params.StackToken.NAME, Package.Events.JTapiHangup.Params.StackToken.DISPLAY, typeof(string), true, Package.Events.JTapiHangup.Params.StackToken.DESCRIPTION)]
        private void PropOnHangup(CallInfo cInfo, string cause, string deviceName)
        {
            // Send the event
            EventMessage eMsg = base.messageUtility.CreateEventMessage(Package.Events.JTapiHangup.FULLNAME, 
                EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            eMsg.AddField(Package.Events.JTapiHangup.Params.CallId.NAME, cInfo.CallId);
            eMsg.AddField(Package.Events.JTapiHangup.Params.DeviceName.NAME, deviceName);
            eMsg.AddField(Package.Events.JTapiHangup.Params.StackToken.NAME, cInfo.StackCallId);
            eMsg.AddField(Package.Events.JTapiHangup.Params.Cause.NAME, cause);
            
            devices.RemoveCall(cInfo.CallId);

            log.Write(TraceLevel.Info, "Sending third-party Hangup for '{0}' (cause={1}) ", cInfo.CallId, cause);
            
            base.palWriter.PostMessage(eMsg);
        }

        [Event(Package.Events.JTapiGotDigits.FULLNAME, false, null, Package.Events.JTapiGotDigits.DISPLAY, Package.Events.JTapiGotDigits.DESCRIPTION)]
        [EventParam(Package.Events.JTapiGotDigits.Params.CallId.NAME, Package.Events.JTapiGotDigits.Params.CallId.DISPLAY, typeof(long), true, Package.Events.JTapiGotDigits.Params.CallId.DESCRIPTION)]
        [EventParam(Package.Events.JTapiGotDigits.Params.DeviceName.NAME, Package.Events.JTapiGotDigits.Params.DeviceName.DISPLAY, typeof(string), true, Package.Events.JTapiGotDigits.Params.DeviceName.DESCRIPTION)]
        [EventParam(Package.Events.JTapiGotDigits.Params.Digits.NAME, Package.Events.JTapiGotDigits.Params.Digits.DISPLAY, typeof(string), true, Package.Events.JTapiGotDigits.Params.Digits.DESCRIPTION)]
        [EventParam(Package.Events.JTapiGotDigits.Params.Source.NAME, Package.Events.JTapiGotDigits.Params.Source.DISPLAY, typeof(string), true, Package.Events.JTapiGotDigits.Params.Source.DESCRIPTION)]
        [EventParam(Package.Events.JTapiGotDigits.Params.StackToken.NAME, Package.Events.JTapiGotDigits.Params.StackToken.DISPLAY, typeof(string), true, Package.Events.JTapiGotDigits.Params.StackToken.DESCRIPTION)]
        private void PropOnGotDigits(CallInfo cInfo, string deviceName, string digits, uint source)
        {
            // Send the event
            EventMessage eMsg = base.messageUtility.CreateEventMessage(Package.Events.JTapiGotDigits.FULLNAME, 
                EventMessage.EventType.NonTriggering, cInfo.RoutingGuid);
            eMsg.AddField(Package.Events.JTapiGotDigits.Params.CallId.NAME, cInfo.CallId);
            eMsg.AddField(Package.Events.JTapiGotDigits.Params.DeviceName.NAME, deviceName);
            eMsg.AddField(Package.Events.JTapiGotDigits.Params.StackToken.NAME, cInfo.StackCallId);
            eMsg.AddField(Package.Events.JTapiGotDigits.Params.Digits.NAME, digits);
            eMsg.AddField(Package.Events.JTapiGotDigits.Params.Source.NAME, ((Source)source).ToString());
            
            log.Write(TraceLevel.Info, "Sending third-party GotDigits for '{0}': {1}", cInfo.CallId, digits);
            
            base.palWriter.PostMessage(eMsg);
        }

        #endregion

        #region CallControl action handlers

        protected override bool HandleMakeCall(OutboundCallInfo callInfo)
        {
            return MakeCall(null, callInfo, true, true);
        }

        private bool MakeCall(CallInfo cInfo, OutboundCallInfo outCallInfo, bool nextMember, bool initial)
        {
            CrgMember member = null;

            if(nextMember)
                member = outCallInfo.RouteGroup.GetNextMember();
            else
                member = outCallInfo.RouteGroup.GetCurrentMember();

            if(member == null)
            {
                log.Write(TraceLevel.Warning, "All CTI devices associated with {0}->{1} are in use or not available",
                    outCallInfo.AppName, outCallInfo.PartitionName);
                return false;
            }

            string deviceName = null;

            // Could be either a device pool or route point
            CtiDevicePool dp = member as CtiDevicePool;
            CtiRoutePoint rp = member as CtiRoutePoint;
            if(dp != null)
            {
                uint deviceIndex;
                if(outCallInfo.DeviceIndex == 0)
                    deviceName = GetFreeDeviceName(dp.DeviceNames, 0, out deviceIndex);
                else
                    deviceName = GetFreeDeviceName(dp.DeviceNames, outCallInfo.DeviceIndex+1, out deviceIndex);

                if(deviceName == null)
                    return MakeCall(cInfo, outCallInfo, true, false);
                else
                    outCallInfo.DeviceIndex = deviceIndex;
            }
            else if(rp != null)
            {
                JTapiDeviceInfo jdInfo = devices.GetDeviceInfo(rp.DeviceName);

                // If this device is no good, try the next
                if (jdInfo == null || jdInfo.DeviceInfo == null || 
                    jdInfo.DeviceInfo.Status != IConfig.Status.Enabled_Running)
                {
                    return MakeCall(cInfo, outCallInfo, true, false);
                }

                deviceName = rp.DeviceName;
            }
            else
            {
                log.Write(TraceLevel.Error, "Invalid CRG member detected for {0}->{1}",
                    outCallInfo.AppName, outCallInfo.PartitionName);
                return MakeCall(cInfo, outCallInfo, true, false);
            }

            if(cInfo == null)
            {
                // The stack will use the call ID we supply for outbound calls (thank gawd!)
                // The from field must be a valid line on the device. Since the app has no
                //   way to know this, specify nothing and let the stack pick a line.
                cInfo = devices.AddCall(outCallInfo.CallId.ToString(), outCallInfo.CallId, deviceName, 
                    outCallInfo.To, null, outCallInfo, CallDirection.Outbound);

                if(cInfo == null)
                {
                    log.Write(TraceLevel.Error, "Could not make new call {0}:{1}", deviceName, outCallInfo.CallId);
                    return false;
                }
            }
            else
            {
                devices.RemoveCall(cInfo.CallId);
                devices.AddCall(deviceName, cInfo);
            }

            cInfo.State = CallState.PendingOutbound;
            cInfo.IsPeerToPeer = outCallInfo.IsPeerToPeer;

            if(initial && outCallInfo.RxAddr == null)
            {
                // Request local media info
                base.SendGotCapabilities(outCallInfo.CallId, null);
                return true;
            }
            else
            {
                return MakeCall(cInfo, deviceName);
            }
        }

        private bool MakeCall(CallInfo cInfo, string deviceName)
        {
            JTapiDeviceInfo jdInfo = devices[deviceName];
            if(jdInfo == null || jdInfo.DeviceInfo == null)
                return false;

            log.Write(TraceLevel.Info, "Placing call to '{0}' from '{1}' (Call ID: {2})", 
                cInfo.To, deviceName, cInfo.CallId);

            return cInfo.Proxy.SendMakeCall(cInfo.StackCallId, cInfo.To, cInfo.From, 
                jdInfo.DeviceInfo.Name, jdInfo.DeviceInfo.Type, cInfo.RxIP, (int)cInfo.RxPort);
        }

        protected override bool HandleAcceptCall(long callId, bool p2p)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to accept a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received AcceptCall for non-existent call: " + callId);

                return false;
            }

            cInfo.IsPeerToPeer = p2p;

            if(cInfo.Error)
                return false;

            return cInfo.Proxy.SendAcceptCall(cInfo.StackCallId);
        }

        protected override bool HandleRedirectCall(long callId, string to)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to redirect a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received RedirectCall for non-existent call: " + callId);

                return false;
            }

            if(cInfo.Error)
                return false;

            bool success = false;
            if(cInfo.State == CallState.Init)
            {
                success = cInfo.Proxy.SendRedirect(cInfo.StackCallId, to);
                devices.RemoveCall(callId);
            }
            else if(cInfo.State == CallState.Active)
            {
                // They meant BlindTransfer, don't play stupid
                success = cInfo.Proxy.SendBlindTransfer(cInfo.StackCallId, to);
                devices.RemoveCall(callId);
            }
            else
            {
                log.Write(TraceLevel.Warning, "Call is not in a state where it call be redirected: {0} (state: {1})",
                    callId.ToString(), cInfo.State.ToString());
            }

            return success;
        }

        protected override bool HandleBlindTransfer(long callId, string to)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to transfer a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received BlindTransfer for non-existent call: " + callId);

                return false;
            }

            if(cInfo.Error)
                return false;

            bool success = false;
            if(cInfo.State == CallState.Init)
            {
                // They meant Redirect, don't play stupid
                success = cInfo.Proxy.SendRedirect(cInfo.StackCallId, to);
                devices.RemoveCall(callId);
            }
            else if(cInfo.State == CallState.Active)
            {
                success = cInfo.Proxy.SendBlindTransfer(cInfo.StackCallId, to);
                devices.RemoveCall(callId);
            }
            else
            {
                log.Write(TraceLevel.Warning, "Call is not in a state where it call be transfered: {0} (state: {1})",
                    callId.ToString(), cInfo.State.ToString());
            }

            return success;
        }

        protected override bool HandleAnswerCall(long callId, string displayName)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to answer a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received AnswerCall for non-existent call: " + callId);

                return false;
            }

            if(cInfo.Error)
                return false;
            
			if(cInfo.State == CallState.Init)
			{
				cInfo.State = CallState.Answered;
				return true;
			}

			if(cInfo.State == CallState.Ringing)
			{
				cInfo.State = CallState.Answered;
				return cInfo.Proxy.SendAnswerCall(cInfo.StackCallId, cInfo.RxIP, (int)cInfo.RxPort);
			}

			log.Write(TraceLevel.Error, "Cannot answer call {0}, invalid state: {1}",
				callId, cInfo.State.ToString());
			return false;
        }

        protected override bool HandleSetMedia(long callId, string rxIP, uint rxPort, string rxControlIP, 
            uint rxControlPort, IMediaControl.Codecs rxCodec, uint rxFramesize, IMediaControl.Codecs txCodec, 
            uint txFramesize, MediaCapsField caps)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received SetMedia for non-existent call: " + callId);
                return false;
            }

            if(cInfo.Error)
                return false;

            // Note: Ignore media caps because JTAPI doesn't support per-call caps exchange.

            // Save media info
            // Set the RxAddr in the callInfo. The string version of Ip and int port
            // will be converted to IPEndpoint.
            cInfo.SetRxAddress(rxIP, rxPort);

            log.Write(TraceLevel.Verbose, "Set media for call (ID: {0}) RxIp: {1} RxPort: {2}", callId, cInfo.RxIP, cInfo.RxPort);

            if(cInfo.State == CallState.PendingOutbound)
            {
                string deviceName = devices.GetDeviceName(callId);                
                if(!MakeCall(cInfo, deviceName))
                {
                    log.Write(TraceLevel.Error, "Failed to place outbound call (ID: {0}): {1}", cInfo.CallId,
                        cInfo.ErrorMessage != null ? cInfo.ErrorMessage : "<no error details>");
                    devices.RemoveCall(cInfo.CallId);
                    base.SendCallSetupFailed(cInfo.CallId, cInfo.EndReason);
                }
            }
            else if(cInfo.State != CallState.Init && cInfo.State != CallState.Ringing)
            {
                // Send new Rx address to JTAPI service if call is established
                return cInfo.Proxy.SendSetMedia(cInfo.StackCallId, cInfo.RxIP, (int)cInfo.RxPort);
            }

            return true;
        }

        protected override bool HandleHold(long callId)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received Hold for non-existent call: " + callId);
                return false;
            }

            if(cInfo.Error)
                return false;

            cInfo.Proxy.SendHold(cInfo.StackCallId);
            return true;
        }

        protected override bool HandleResume(long callId, string rxIP, uint rxPort, string rxControlIP, uint rxControlPort)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received Resume for non-existent call: " + callId);
                return false;
            }

            if(cInfo.Error)
                return false;


            // Save media info
            // Set the RxAddr in the callInfo. The string version of Ip and int port
            // will be converted to IPEndpoint.
            cInfo.SetRxAddress(rxIP, rxPort);

            log.Write(TraceLevel.Verbose, "Set media for resumed call (ID: {0}) RxIp: {1} RxPort: {2}", callId, cInfo.RxIP, cInfo.RxPort);

            cInfo.Proxy.SendResume(cInfo.StackCallId, cInfo.RxIP, cInfo.RxPort);
            return true;
        }

        protected override bool HandleUseMohMedia(long callId)
        { 
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    return true;                // Just smile and nod

                log.Write(TraceLevel.Warning, "Received UseMusicOnHoldMedia for non-existent call: " + callId);
                return false;
            }

            return cInfo.Proxy.SendUseMohMedia(cInfo.StackCallId);
        }


        protected override bool HandleRejectCall(long callId)
        {
            return HandleTerminateCall(callId);
        }

        protected override bool HandleHangup(long callId)
        {
            return HandleTerminateCall(callId);
        }

        protected override bool HandleSendUserInput(long callId, string digits)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(callId))
                    log.Write(TraceLevel.Info, "An attempt has been made to send digits on a call which has already ended: " + callId);
                else
                    log.Write(TraceLevel.Warning, "Received SendUserInput for non-existent call: " + callId);

                return false;
            }

            return cInfo.Proxy.SendUserInput(cInfo.StackCallId, digits);
        }


        private bool HandleTerminateCall(long callId)
        {
            CallInfo cInfo = devices.GetCall(callId);
            if(cInfo != null)
            {
                if(cInfo.State == CallState.Init)
                    cInfo.Proxy.SendRejectCall(cInfo.StackCallId);
                else
                    cInfo.Proxy.SendHangup(cInfo.StackCallId);

                devices.RemoveCall(callId);
            }
            return true;
        }

        #endregion

        #region CallControl event senders

        private void OnError(string deviceName, string stackCallId, JTapiProxy.FailReason reason, 
            string message, JTapiProxy.MsgType msgType, JTapiProxy.MsgField msgField)
        {
            // Format and print an error message
            string title = "<Missing ID info>";
            if(deviceName != null && deviceName != String.Empty)
            {
                if(stackCallId == null)
                    title = String.Format("Device {0}", deviceName);
                else
                    title = String.Format("Call {0}:{1}", deviceName, stackCallId);
            }
            else if(stackCallId != null)
                title = String.Format("Call {0}", stackCallId);

            string errorMsg = String.Format("{0} encountered an error ({1}): {2}",
                title, reason.ToString(), message != null ? message : "<no description>");

            // Terminate the call
            if(stackCallId != null && IsFatalError(message))
            {
                CallInfo cInfo = devices.GetCall(stackCallId);
                if(cInfo != null)
                {
                    cInfo.ErrorMessage = errorMsg;

                    if(cInfo.State == CallState.PendingOutbound)
                    {
                        deviceName = devices.GetDeviceName(cInfo.CallId);
                        log.Write(TraceLevel.Warning, "Outbound call {0}:{1} failed ({2}). Attempting failover...", 
                            deviceName != null ? deviceName : "(unknown)", cInfo.CallId, reason.ToString());

                        if(!Failover(cInfo, reason))
                        {
                            devices.RemoveCall(cInfo.CallId);
                            base.SendCallSetupFailed(cInfo.CallId, cInfo.EndReason);
                        }
                    }
                    else
                    {
                        devices.RemoveCall(cInfo.CallId);
                        base.SendHangup(cInfo.CallId, ICallControl.EndReason.InternalError);
                    }
                }
            }
            
            log.Write(TraceLevel.Error, errorMsg);
        }

        private bool Failover(CallInfo cInfo, JTapiProxy.FailReason reason)
        {
            // Figure out the CC end reason
            switch(reason)
            {
                case JTapiProxy.FailReason.NoProvider:
                case JTapiProxy.FailReason.InvalidDeviceType:
                case JTapiProxy.FailReason.CodecNotSupported:
                    cInfo.EndReason = ICallControl.EndReason.InternalError;
                    return FailoverToNextMember(cInfo);
                case JTapiProxy.FailReason.InvalidDN:
                    cInfo.EndReason = ICallControl.EndReason.Unreachable;
                    return FailoverToNextMember(cInfo);
                case JTapiProxy.FailReason.MissingField:
                case JTapiProxy.FailReason.UnknownMessageType:
                case JTapiProxy.FailReason.InvalidDestination:
                case JTapiProxy.FailReason.PlatformException:
                    // Do not attempt failover in these cases
                    cInfo.EndReason = ICallControl.EndReason.InternalError;
                    return false;
                default:
                    cInfo.EndReason = ICallControl.EndReason.InternalError;
                    return FailoverWithinCurrentPool(cInfo);
            }
        }

        private bool FailoverToNextMember(CallInfo cInfo)
        {
            // Try to fail to another pool or route point
            return MakeCall(cInfo, (OutboundCallInfo)cInfo, true, false);
        }

        private bool FailoverWithinCurrentPool(CallInfo cInfo)
        {
            string deviceName = devices.GetDeviceName(cInfo.CallId);
            JTapiDeviceInfo jdInfo = devices[deviceName];
            if (jdInfo != null && jdInfo.DeviceInfo != null && 
                jdInfo.DeviceInfo.Type == IConfig.DeviceType.CtiPort)
            {
                // Try to fail over to another device in the group
                return MakeCall(cInfo, (OutboundCallInfo)cInfo, false, false);
            }
            else  // Route point
            {
                return FailoverToNextMember(cInfo);
            }
        }

        private bool IsFatalError(string message)
        {
            if(message == null || message == String.Empty)
                return true;

            // Parse off any elaboration from the error message
            message = message.Split(':')[0];

            return !nonFatalErrors.Contains(message);
        }

        private void OnMakeCallAck(string stackCallId)
        {
            // Do nothing
        }

        private void OnRinging(string stackCallId)
        {
            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received Ringing for non-existent call: " + stackCallId);
                }
                return;
            }
            
            if(cInfo.Error)
                return;
            else if(cInfo.State == CallState.Init)
                cInfo.State = CallState.Ringing;
            else if(cInfo.State == CallState.Answered)
            {
                string deviceName = devices.GetDeviceName(stackCallId);
                if(deviceName == null)
                {
                    log.Write(TraceLevel.Error, "Internal error: No device associated with call '{0}'", stackCallId);
                    return;
                }

                if(devices.IsThirdParty(deviceName))
                    cInfo.Proxy.SendAnswerCall(cInfo.StackCallId);
                else
                    cInfo.Proxy.SendAnswerCall(cInfo.StackCallId, cInfo.RxIP, (int)cInfo.RxPort);
            }
        }

        private void OnStatusUpdate(string deviceName, string dn, IConfig.Status status)
        {
            JTapiDeviceInfo jdInfo = devices[deviceName];
            if(jdInfo == null || jdInfo.DeviceInfo == null)
            {
                log.Write(TraceLevel.Error, "Received StatusUpdate for unknown device: " + deviceName);
                return;
            }

            // If no change, ignore it.
            if(jdInfo.DeviceInfo.DirectoryNumber == dn && jdInfo.DeviceInfo.Status == status)
                return;

            if(status == IConfig.Status.Unspecified || status == IConfig.Status.Disabled)
            {
                log.Write(TraceLevel.Error, "JTAPI service attempting to set device {0}'s status to: {1}", 
                    deviceName, status.ToString());
                return;
            }
            else if(status == IConfig.Status.Disabled_Error)
            {
                log.Write(TraceLevel.Warning, "Device '{0}' has encountered an unrecoverable error.", deviceName);
            }
            else if(status == IConfig.Status.Enabled_Running)
            {
                log.Write(TraceLevel.Info, "Device '{0}' registered successfully.", deviceName);
            }

            devices.SetStatus(deviceName, status);

            // Record the DN in the database for admin purposes
            configUtility.SetDirectoryNumber(deviceName, jdInfo.DeviceInfo.Type, dn);
        }

        private void OnCallEstablished(string stackCallId, string from, string to, string originalTo)
        {
            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received CallEstablished message for non-existent call: " + stackCallId);
                }
                return;
            }

            cInfo.To = to;
            cInfo.From = from;

            string deviceName = devices.GetDeviceName(cInfo.CallId);
            Assertion.Check(deviceName != null, "Valid call not associated with any device: " + cInfo.CallId);

            if(devices.IsThirdParty(deviceName))
            {
                if(cInfo.Direction == CallDirection.Inbound || cInfo.State != CallState.Init)
                    PropOnCallActive(cInfo, deviceName);
                else
                    PropOnCallEstablished(cInfo, deviceName);
            }
            else if(cInfo.State == CallState.Active)
            {
                base.SendCallChanged(cInfo.CallId, cInfo.To, cInfo.From);
            }
            else if(cInfo.Direction == CallDirection.Inbound)
            {
                // Ignore this message for outbound calls and wait for Answered instead.
                base.SendCallEstablished(cInfo.CallId, cInfo.To, cInfo.From);
                cInfo.State = CallState.Active;
            }
        }

        private void OnAnswered(string stackCallId, string from, string to, string originalTo)
        {
            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received Answered message for non-existent call: " + stackCallId);
                }
                return;
            }

			lock(cInfo.CallLock)
			{
				cInfo.To = to;

				string deviceName = devices.GetDeviceName(cInfo.CallId);
				Assertion.Check(deviceName != null, "Valid call not associated with any device: " + cInfo.CallId);

				// This message should only ever arrive for outbound calls
				if(cInfo.Direction == CallDirection.Outbound)
				{
					if(devices.IsThirdParty(deviceName))
					{
						PropOnCallActive(cInfo, deviceName);
					}
					else 
					{
						base.SendCallEstablished(cInfo.CallId, cInfo.To, cInfo.From);
						cInfo.State = CallState.Active;
					}
				}
			}
        }

        private void OnMediaEstablished(string stackCallId, IMediaControl.Codecs codec, 
            uint framesize, string txIP, ushort txPort)
        {
            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received MediaEstablished message for non-existent call: " + stackCallId);
                }
                return;
            }

			lock(cInfo.CallLock)
			{
				string deviceName = devices.GetDeviceName(cInfo.CallId);
				if (deviceName == null)
				{
					log.Write(TraceLevel.Warning, "Valid call not associated with any device: " + cInfo.CallId);
					cInfo.Proxy.SendHangup(stackCallId);
					return;
				}

				// Ignore this message for third-party calls
				if(devices.IsThirdParty(deviceName) == false)
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
        }

        private void OnIncomingCall(string stackCallId, string deviceName, string to, string from, string originalTo)
        {
            long callId = base.callIdFactory.GenerateCallId();

			if (devices.AddCall(stackCallId, callId, deviceName, to, from, null, CallDirection.Inbound) == null)
			{
				log.Write(TraceLevel.Error, "Could not create new incoming call {0} on unknown device {1}", stackCallId, deviceName);
				return;
			}

            // Is it first or third party?
            if(devices.IsThirdParty(deviceName))
            {
                PropOnIncomingCall(callId, stackCallId, deviceName, to, from, originalTo);
            }
            else
            {
                base.SendIncomingCall(callId, stackCallId, to, from, originalTo, false, from);
            }
        }

        private void OnHangup(string stackCallId, string cause)
        {
            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo != null)
            {
				lock(cInfo.CallLock)
				{
					// Is it first or third party?
					string deviceName = devices.GetDeviceName(stackCallId);
					if (deviceName == null)
						return;

					devices.RemoveCall(cInfo.CallId);

					if(devices.IsThirdParty(deviceName))
					{
						PropOnHangup(cInfo, cause, deviceName);
					}
					else if(cInfo.Direction == CallDirection.Outbound && 
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

        private void OnGotDigits(string stackCallId, string digits, uint source)
        {
            CallInfo cInfo = devices.GetCall(stackCallId);
            if(cInfo == null)
            {
                if(devices.IsRecentlyEnded(stackCallId) == false)
                {
                    log.Write(TraceLevel.Warning, "Received GotDigits message for non-existent call: " + stackCallId);
                }
                return;
            }

			lock(cInfo.CallLock)
			{
				log.Write(TraceLevel.Verbose, "Got digit(s) '{0}' in call: {1}", digits, cInfo.CallId);

				string deviceName = devices.GetDeviceName(stackCallId);
				if(deviceName == null)
				{
					log.Write(TraceLevel.Error, "Internal error: No device associated with call '{0}'", stackCallId);
					return;
				}

				if(devices.IsThirdParty(deviceName))
				{
					PropOnGotDigits(cInfo, deviceName, digits, source);
				}
				else if((Source)source == Source.Remote)  // Ignore local digit events
				{
					base.SendGotDigits(cInfo.CallId, digits);
				}
			}
        }
        #endregion

        #region Private helper methods

        /// <summary>Gets the set of all CTI devices in the AppServer database</summary>
        /// <returns>DeviceInfo Array</returns>
        private ArrayList GetCtiDevicesFromDb()
        {
            ArrayList dbDevices = new ArrayList();

            ComponentInfo[] poolGroups = configUtility.GetComponents(IConfig.ComponentType.CTI_DevicePool);
            if(poolGroups != null)
            {
                foreach(ComponentInfo cInfo in poolGroups)
                {
                    DeviceInfo[] devs = configUtility.GetCtiDevices(cInfo);
                    if(devs != null) 
                        dbDevices.AddRange(devs);
                }
            }

            ComponentInfo[] routeGroups = configUtility.GetComponents(IConfig.ComponentType.CTI_RoutePoint);
            if(routeGroups != null)
            {
                foreach(ComponentInfo cInfo in routeGroups)
                {
                    DeviceInfo[] devs = configUtility.GetCtiDevices(cInfo);
                    if(devs != null) 
                        dbDevices.AddRange(devs);
                }
            }

            ComponentInfo[] monitoredGroups = configUtility.GetComponents(IConfig.ComponentType.CTI_Monitored);
            if(monitoredGroups != null)
            {
                foreach(ComponentInfo cInfo in monitoredGroups)
                {
                    DeviceInfo[] devs = configUtility.GetCtiDevices(cInfo);
                    if(devs != null) 
                        dbDevices.AddRange(devs);
                }
            }

            return dbDevices;
        }

        private string GetFreeDeviceName(StringCollection deviceNames, uint startIndex, out uint deviceIndex)
        {
            for(deviceIndex = startIndex; deviceIndex < deviceNames.Count; deviceIndex++)
            {
                string deviceName = deviceNames[(int)deviceIndex];
                if(devices.InUse(deviceName) == false)
                    return deviceName;
            }
            return null;
        }

        #endregion
    }
}
