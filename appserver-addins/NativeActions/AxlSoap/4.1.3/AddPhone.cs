using System;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Net;
using System.Web;
using System.Web.Services.Protocols;

using System.Data;
using System.Collections;
using System.Diagnostics;

using Metreos.Core;
using Metreos.Interfaces;
using Metreos.LoggingFramework;
using Metreos.ApplicationFramework;
using Metreos.PackageGeneratorCore.Attributes;
using Metreos.ApplicationFramework.Collections;

using Metreos.AxlSoap;
using Metreos.AxlSoap413;
using Metreos.Types.AxlSoap413;

namespace Metreos.Native.AxlSoap413
{
	/// <summary> Wraps up the 'addPhone.newPhone' AXL SOAP method for Cisco CallManager 4.1.3 </summary>
    [PackageDecl("Metreos.Native.AxlSoap413")]
    public class AddPhone : INativeAction
	{
        [ActionParamField(Package.Params.BuiltInBridgeStatus.DISPLAY, Package.Params.BuiltInBridgeStatus.DESCRIPTION, false, Package.Params.BuiltInBridgeStatus.DEFAULT)]
        public XStatus BuiltInBridgeStatus { set { builtInBridgeStatus = value; builtInBridgeStatusSpecified = true; } }
        
        [ActionParamField(Package.Params.CallInfoPrivacyStatus.DISPLAY, Package.Params.CallInfoPrivacyStatus.DESCRIPTION, false, Package.Params.CallInfoPrivacyStatus.DEFAULT)]
        public XStatus CallInfoPrivacyStatus { set { callInfoPrivacyStatus = value; callInfoPrivacyStatusSpecified = true;} }

        [ActionParamField(Package.Params.Description.DISPLAY, Package.Params.Description.DESCRIPTION, false, Package.Params.Description.DEFAULT)]
        public string Description { set { description = value; } }

        [ActionParamField(Package.Params.DeviceSecurityMode.DISPLAY, Package.Params.DeviceSecurityMode.DESCRIPTION, false, Package.Params.DeviceSecurityMode.DEFAULT)]
        public XDeviceSecurityMode DeviceSecurityMode { set { deviceSecurityMode = value; deviceSecurityModeSpecified = true; } }

        [ActionParamField(Package.Params.EnableExtensionMobility.DISPLAY, Package.Params.EnableExtensionMobility.DESCRIPTION, false, Package.Params.EnableExtensionMobility.DEFAULT)]
        public bool EnableExtensionMobility { set { enableExtensionMobility = value; enableExtensionMobilitySpecified = true;} }

        [ActionParamField(Package.Params.AssociatedPC.DISPLAY, Package.Params.AssociatedPC.DESCRIPTION, false, Package.Params.AssociatedPC.DEFAULT)]
        public string AssociatedPC { set { associatedPC = value; } }

        [ActionParamField(Package.Params.AuthenticationUrl.DISPLAY, Package.Params.AuthenticationUrl.DESCRIPTION, false, Package.Params.AuthenticationUrl.DEFAULT)]
        public string AuthenticationUrl { set { authenticationUrl = value; } }

        [ActionParamField(Package.Params.DirectoryUrl.DISPLAY, Package.Params.DirectoryUrl.DESCRIPTION, false, Package.Params.DirectoryUrl.DEFAULT)]
        public string DirectoryUrl { set { directoryUrl = value; } }

        [ActionParamField(Package.Params.IdleTimeout.DISPLAY, Package.Params.IdleTimeout.DESCRIPTION, false, Package.Params.IdleTimeout.DEFAULT)]
        public string IdleTimeout { set { idleTimeout = value; } }

        [ActionParamField(Package.Params.IdleUrl.DISPLAY, Package.Params.IdleUrl.DESCRIPTION, false, Package.Params.IdleUrl.DEFAULT)]
        public string IdleUrl { set { idleUrl = value; } }

        [ActionParamField(Package.Params.InformationUrl.DISPLAY, Package.Params.InformationUrl.DESCRIPTION, false, Package.Params.InformationUrl.DEFAULT)]
        public string InformationUrl { set { informationUrl = value; } }

        [ActionParamField(Package.Params.PhoneName.DISPLAY, Package.Params.PhoneName.DESCRIPTION, true, Package.Params.PhoneName.DEFAULT)]
        public string PhoneName { set { phoneName = value; } }

        [ActionParamField(Package.Params.CallingSearchSpaceName.DISPLAY, Package.Params.CallingSearchSpaceName.DESCRIPTION, false, Package.Params.CallingSearchSpaceName.DEFAULT)]
        public string CallingSearchSpaceName { set { callingSearchSpaceName = value; } }
        
        [ActionParamField(Package.Params.CallingSearchSpaceId.DISPLAY, Package.Params.CallingSearchSpaceId.DESCRIPTION, false, Package.Params.CallingSearchSpaceId.DEFAULT)]
        public string CallingSearchSpaceId { set { callingSearchSpaceId = value; } }
        
        [ActionParamField(Package.Params.DevicePoolName.DISPLAY, Package.Params.DevicePoolName.DESCRIPTION, false, Package.Params.DevicePoolName.DEFAULT)]
        public string DevicePoolName { set { devicePoolName = value; } }
        
        [ActionParamField(Package.Params.DevicePoolId.DISPLAY, Package.Params.DevicePoolId.DESCRIPTION, false, Package.Params.DevicePoolId.DEFAULT)]
        public string DevicePoolId { set { devicePoolId = value; } }
        
        [ActionParamField(Package.Params.LocationName.DISPLAY, Package.Params.LocationName.DESCRIPTION, false, Package.Params.LocationName.DEFAULT)]
        public string LocationName { set { locationName = value; } }
        
        [ActionParamField(Package.Params.LocationId.DISPLAY, Package.Params.LocationId.DESCRIPTION, false, Package.Params.LocationId.DEFAULT)]
        public string LocationId { set { locationId = value; } }
        
        [ActionParamField(Package.Params.MediaResourceListName.DISPLAY, Package.Params.MediaResourceListName.DESCRIPTION, false, Package.Params.MediaResourceListName.DEFAULT)]
        public string MediaResourceListName { set { mediaResourceListName = value; } }
        
        [ActionParamField(Package.Params.MediaResourceListId.DISPLAY, Package.Params.MediaResourceListId.DESCRIPTION, false, Package.Params.MediaResourceListId.DEFAULT)]
        public string MediaResourceListId { set { mediaResourceListId = value; } }
        
        [ActionParamField(Package.Params.AutomatedAlternateRoutingCSSName.DISPLAY, Package.Params.AutomatedAlternateRoutingCSSName.DESCRIPTION, false, Package.Params.AutomatedAlternateRoutingCSSName.DEFAULT)]
        public string AutomatedAlternateRoutingCSSName { set { automatedAlternateRoutingCSSName = value; } }
        
        [ActionParamField(Package.Params.AutomatedAlternateRoutingCSSId.DISPLAY, Package.Params.AutomatedAlternateRoutingCSSId.DESCRIPTION, false, Package.Params.AutomatedAlternateRoutingCSSId.DEFAULT)]
        public string AutomatedAlternateRoutingCSSId { set { automatedAlternateRoutingCSSId = value; } }
        
        [ActionParamField(Package.Params.AarNeighborhoodName.DISPLAY, Package.Params.AarNeighborhoodName.DESCRIPTION, false, Package.Params.AarNeighborhoodName.DEFAULT)]
        public string AarNeighborhoodName { set { aarNeighborhoodName = value; } }
        
        [ActionParamField(Package.Params.AarNeighborhoodId.DISPLAY, Package.Params.AarNeighborhoodId.DESCRIPTION, false, Package.Params.AarNeighborhoodId.DEFAULT)]
        public string AarNeighborhoodId { set { aarNeighborhoodId = value; } }
        
        [ActionParamField(Package.Params.PhoneTemplateName.DISPLAY, Package.Params.PhoneTemplateName.DESCRIPTION, false, Package.Params.PhoneTemplateName.DEFAULT)]
        public string PhoneTemplateName { set { phoneTemplateName = value; } }
        
        [ActionParamField(Package.Params.PhoneTemplateId.DISPLAY, Package.Params.PhoneTemplateId.DESCRIPTION, false, Package.Params.PhoneTemplateId.DEFAULT)]
        public string PhoneTemplateId { set { phoneTemplateId = value; } }
        
        [ActionParamField(Package.Params.SoftkeyTemplateName.DISPLAY, Package.Params.SoftkeyTemplateName.DESCRIPTION, false, Package.Params.SoftkeyTemplateName.DEFAULT)]
        public string SoftkeyTemplateName { set { softkeyTemplateName = value; } }
        
        [ActionParamField(Package.Params.SoftkeyTemplateId.DISPLAY, Package.Params.SoftkeyTemplateId.DESCRIPTION, false, Package.Params.SoftkeyTemplateId.DEFAULT)]
        public string SoftkeyTemplateId { set { softkeyTemplateId = value; } }
        
        [ActionParamField(Package.Params.DefaultProfileName.DISPLAY, Package.Params.DefaultProfileName.DESCRIPTION, false, Package.Params.DefaultProfileName.DEFAULT)]
        public string DefaultProfileName { set { defaultProfileName = value; } }
        
        [ActionParamField(Package.Params.DefaultProfileId.DISPLAY, Package.Params.DefaultProfileId.DESCRIPTION, false, Package.Params.DefaultProfileId.DEFAULT)]
        public string DefaultProfileId { set { defaultProfileId = value; } }
        
		[ActionParamField(Package.Params.CurrentProfileName.DISPLAY, Package.Params.CurrentProfileName.DESCRIPTION, false, Package.Params.CurrentProfileName.DEFAULT)]
		public string CurrentProfileName { set { currentProfileName = value; } }
        
		[ActionParamField(Package.Params.CurrentProfileId.DISPLAY, Package.Params.CurrentProfileId.DESCRIPTION, false, Package.Params.CurrentProfileId.DEFAULT)]
		public string CurrentProfileId { set { currentProfileId = value; } }
        
        [ActionParamField(Package.Params.LoadInfoSpecial.DISPLAY, Package.Params.LoadInfoSpecial.DESCRIPTION, false, Package.Params.LoadInfoSpecial.DEFAULT)]
        public bool LoadInfoSpecial { set { loadInfoSpecial = value; } }
        
        [ActionParamField(Package.Params.LoadInfoValue.DISPLAY, Package.Params.LoadInfoValue.DESCRIPTION, false, Package.Params.LoadInfoValue.DEFAULT)]
        public string LoadInfoValue { set { loadInfoValue = value; } }

		[ActionParamField(Package.Params.MessageUrl.DISPLAY, Package.Params.MessageUrl.DESCRIPTION, false, Package.Params.MessageUrl.DEFAULT)]
		public string MessageUrl { set { messageUrl = value; } }

        [ActionParamField(Package.Params.MlppDomainId.DISPLAY, Package.Params.MlppDomainId.DESCRIPTION, false, Package.Params.MlppDomainId.DEFAULT)]
        public int MlppDomainId { set { mlppDomainId = value; mlppDomainIdSpecified = true;} }

        [ActionParamField(Package.Params.MlppIndicationStatus.DISPLAY, Package.Params.MlppIndicationStatus.DESCRIPTION, false, Package.Params.MlppIndicationStatus.DEFAULT)]
        public XStatus MlppIndicationStatus { set { mlppIndicationStatus = value; mlppIndicationStatusSpecified = true; } }

        [ActionParamField(Package.Params.NetworkHoldMOHAudioSourceId.DISPLAY, Package.Params.NetworkHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.NetworkHoldMOHAudioSourceId.DEFAULT)]
        public string NetworkHoldMOHAudioSourceId { set { networkHoldMOHAudioSourceId = value; } }

		[ActionParamField(Package.Params.OwnerUserId.DISPLAY, Package.Params.OwnerUserId.DESCRIPTION, false, Package.Params.OwnerUserId.DEFAULT)]
		public string OwnerUserId { set { ownerUserId = value; } }

        [ActionParamField(Package.Params.Preemption.DISPLAY, Package.Params.Preemption.DESCRIPTION, false, Package.Params.Preemption.DEFAULT)]
        public XPreemption Preemption { set { preemption = value; preemptionSpecified = true;} }
       
        [ActionParamField(Package.Params.ProxyServerUrl.DISPLAY, Package.Params.ProxyServerUrl.DESCRIPTION, false, Package.Params.ProxyServerUrl.DEFAULT)]
        public string ProxyServerUrl { set { proxyServerUrl = value; } }
       
        [ActionParamField(Package.Params.ServicesUrl.DISPLAY, Package.Params.ServicesUrl.DESCRIPTION, false, Package.Params.ServicesUrl.DEFAULT)]
        public string ServicesUrl { set { servicesUrl = value; } }
       
        [ActionParamField(Package.Params.TraceFlag.DISPLAY, Package.Params.TraceFlag.DESCRIPTION, false, Package.Params.TraceFlag.DEFAULT)]
        public bool TraceFlag { set { traceFlag = value; traceFlagSpecified = true;} }
       
        [ActionParamField(Package.Params.UserHoldMOHAudioSourceId.DISPLAY, Package.Params.UserHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.UserHoldMOHAudioSourceId.DEFAULT)]
        public string UserHoldMOHAudioSourceId { set { userHoldMOHAudioSourceId = value; } }
        
        [ActionParamField(Package.Params.Speeddials.DISPLAY, Package.Params.Speeddials.DESCRIPTION, false, Package.Params.Speeddials.DEFAULT)]
        public Speeddials Speeddials { set { speeddials = value; } }
        
        [ActionParamField(Package.Params.Services.DISPLAY, Package.Params.Services.DESCRIPTION, false, Package.Params.Services.DEFAULT)]
        public Services Services { set { services = value; } }

		[ActionParamField(Package.Params.Lines.DISPLAY, Package.Params.Lines.DESCRIPTION, false, Package.Params.Lines.DEFAULT)]
		public Lines Lines { set { lines = value; } }

		[ActionParamField(Package.Params.Product.DISPLAY, Package.Params.Product.DESCRIPTION, true, Package.Params.Product.DEFAULT)]
		public XProduct Product { set { product = value; } }

		[ActionParamField(Package.Params.Model.DISPLAY, Package.Params.Model.DESCRIPTION, true, Package.Params.Model.DEFAULT)]
		public XModel Model { set { model = value; } }

		[ActionParamField(Package.Params.Class.DISPLAY, Package.Params.Class.DESCRIPTION, true, Package.Params.Class.DEFAULT)]
		public XClass Class { set { @class = value; } }

		[ActionParamField(Package.Params.Protocol.DISPLAY, Package.Params.Protocol.DESCRIPTION, true, Package.Params.Protocol.DEFAULT)]
		public XDeviceProtocol Protocol { set { protocol = value; } }

		[ActionParamField(Package.Params.ProtocolSide.DISPLAY, Package.Params.ProtocolSide.DESCRIPTION, true, Package.Params.ProtocolSide.DEFAULT)]
		public XProtocolSide ProtocolSide { set { protocolSide = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.AddPhoneResponse.DISPLAY, Package.Results.AddPhoneResponse.DESCRIPTION)]
        public addPhoneResponse AddPhoneResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private XStatus builtInBridgeStatus;
        private bool builtInBridgeStatusSpecified;
        private XStatus callInfoPrivacyStatus;
        private bool callInfoPrivacyStatusSpecified;
        private string description;
        private XDeviceSecurityMode deviceSecurityMode;
        private bool deviceSecurityModeSpecified;
        private bool enableExtensionMobility;
        private bool enableExtensionMobilitySpecified;
        private string associatedPC;
        private string authenticationUrl;
		private XClass @class;
		private string directoryUrl;
		private string idleTimeout;
        private string idleUrl;
        private string informationUrl;
        private string phoneName;
        private string callingSearchSpaceName;
        private string callingSearchSpaceId;
        private string devicePoolName;
        private string devicePoolId;
        private string locationName;
        private string locationId;
        private string mediaResourceListName;
        private string mediaResourceListId;
        private string automatedAlternateRoutingCSSName;
        private string automatedAlternateRoutingCSSId;
        private string aarNeighborhoodName;
        private string aarNeighborhoodId;
		private string ownerUserId;
        private string phoneTemplateName;
        private string phoneTemplateId;
		private XDeviceProtocol protocol;
		private XProtocolSide protocolSide;
        private string softkeyTemplateName;
        private string softkeyTemplateId;
        private string defaultProfileName;
        private string defaultProfileId;
		private string currentProfileName;
		private string currentProfileId;
        private bool loadInfoSpecial;
        private string loadInfoValue;
        private string messageUrl;
        private int mlppDomainId;
        private bool mlppDomainIdSpecified;
        private XStatus mlppIndicationStatus;
        private bool mlppIndicationStatusSpecified;
        private string networkHoldMOHAudioSourceId;
        private XPreemption preemption;
        private bool preemptionSpecified;
        private string proxyServerUrl;
        private string servicesUrl;
        private bool traceFlag;
        private bool traceFlagSpecified;
        private string userHoldMOHAudioSourceId;
        private Services services;
        private Speeddials speeddials;
        private Lines lines;
        private string password;
        private string message;
        private string callManagerIP;
        private string username;
        private int code;
        private LogWriter log;
        private addPhoneResponse response;
		private XProduct product;
		private XModel model;

        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
            
            addPhone addPhone = new addPhone();
			addPhone.newPhone = new XIPPhone();
            addPhone.newPhone.associatedPC = associatedPC;
            addPhone.newPhone.authenticationURL = authenticationUrl;
            addPhone.newPhone.builtInBridgeStatus = builtInBridgeStatus;
            addPhone.newPhone.builtInBridgeStatusSpecified = builtInBridgeStatusSpecified;
            addPhone.newPhone.callInfoPrivacyStatus = callInfoPrivacyStatus;
            addPhone.newPhone.callInfoPrivacyStatusSpecified = callInfoPrivacyStatusSpecified;
			addPhone.newPhone.@class = @class;
			addPhone.newPhone.ownerUserId = ownerUserId;
            addPhone.newPhone.description = description;
            addPhone.newPhone.deviceSecurityMode = deviceSecurityMode;
            addPhone.newPhone.deviceSecurityModeSpecified = deviceSecurityModeSpecified;
            addPhone.newPhone.directoryURL = directoryUrl;
            addPhone.newPhone.enableExtensionMobility = enableExtensionMobility;
            addPhone.newPhone.enableExtensionMobilitySpecified = enableExtensionMobilitySpecified;
            addPhone.newPhone.idleTimeout = idleTimeout;
            addPhone.newPhone.idleURL = idleUrl;
            addPhone.newPhone.informationURL = informationUrl;
			addPhone.newPhone.name = phoneName;
			addPhone.newPhone.Item = product;
			addPhone.newPhone.protocol = protocol;
			addPhone.newPhone.protocolSide = protocolSide;
			addPhone.newPhone.Item1 = model;

			if(callingSearchSpaceId != null)
			{
				XCallingSearchSpace css = new XCallingSearchSpace();
				css.uuid = callingSearchSpaceId;
				addPhone.newPhone.Item2 = css;
			}
			else
			{
				addPhone.newPhone.Item2 = callingSearchSpaceName;
			}

			if(devicePoolId != null)
			{
				XDevicePool dp = new XDevicePool();
				dp.uuid = devicePoolId;
				addPhone.newPhone.Item3 = dp;
			}
			else
			{
				addPhone.newPhone.Item3 = devicePoolName;
			}

			if(locationId != null)
			{
				XLocation location = new XLocation();
				location.uuid = locationId;
				addPhone.newPhone.Item4 = location;
			}
			else
			{
				addPhone.newPhone.Item4 = locationName;
			}

			if(mediaResourceListId != null)
			{
				XMediaResourceList mrl = new XMediaResourceList();
				mrl.uuid = mediaResourceListId;
				addPhone.newPhone.Item5 = mrl;
			}
			else
			{
				addPhone.newPhone.Item5 = mediaResourceListName;
			}

			if(automatedAlternateRoutingCSSId != null)
			{
				XCallingSearchSpace css = new XCallingSearchSpace();
				css.uuid = automatedAlternateRoutingCSSId;
				addPhone.newPhone.Item6 = css;
			}
			else
			{
				addPhone.newPhone.Item6 = automatedAlternateRoutingCSSName;
			}

			if(aarNeighborhoodId != null)
			{
				XAARNeighborhood aar = new XAARNeighborhood();
				aar.uuid = aarNeighborhoodId;
				addPhone.newPhone.Item7 = aar;
			}
			else
			{
				addPhone.newPhone.Item7 = aarNeighborhoodName;
			}

			if(phoneTemplateId != null)
			{
				XPhoneTemplate pt = new XPhoneTemplate();
				pt.uuid = phoneTemplateId;
				addPhone.newPhone.Item8 = pt;
			}
			else
			{
				addPhone.newPhone.Item8 = phoneTemplateName;
			}

			if(softkeyTemplateId != null)
			{
				XSoftkeyTemplate skt = new XSoftkeyTemplate();
				skt.uuid = softkeyTemplateId;
				addPhone.newPhone.Item9 = skt;
			}
			else
			{
				addPhone.newPhone.Item9 = softkeyTemplateName;
			}

			if(defaultProfileId != null)      // Used in conjunction with Extension Mobility

			{
				XProfile profile = new XProfile();
				profile.uuid = defaultProfileId;
				addPhone.newPhone.Item10 = profile;
			}
			else
			{
				addPhone.newPhone.Item10 = defaultProfileName;
			}

			if(currentProfileId != null)      // Used in conjunction with Extension Mobility

			{
				XProfile profile = new XProfile();
				profile.uuid = currentProfileId;
				addPhone.newPhone.Item11 = profile;
			}
			else
			{
				addPhone.newPhone.Item11 = currentProfileName;
			}

            addPhone.newPhone.lines = BuildLines();

            XLoadInformation loadInfo = new XLoadInformation();
            loadInfo.special = loadInfoSpecial;
            loadInfo.specialSpecified = true;
            loadInfo.Value = loadInfoValue; // This will be a flag to say if its specified

            addPhone.newPhone.loadInformation = loadInfo.Value == null ? null : loadInfo;
            addPhone.newPhone.messagesURL = messageUrl;
            addPhone.newPhone.mlppDomainId = mlppDomainId;
            addPhone.newPhone.mlppDomainIdSpecified = mlppDomainIdSpecified;
            addPhone.newPhone.mlppIndicationStatus = mlppIndicationStatus;
            addPhone.newPhone.mlppIndicationStatusSpecified = mlppIndicationStatusSpecified;
            addPhone.newPhone.networkHoldMOHAudioSourceId = networkHoldMOHAudioSourceId;
            addPhone.newPhone.preemption = preemption;
            addPhone.newPhone.preemptionSpecified = preemptionSpecified;
            addPhone.newPhone.proxyServerURL = proxyServerUrl;
			addPhone.newPhone.protocol = protocol;
			addPhone.newPhone.protocolSide = protocolSide;

			// MSC: Why does UpdatePhone have this and AddPhone doesn't?
            //addPhone.newPhone.retryVideoCallAsVoice = retryVideoCallAsVoice;
            //addPhone.newPhone.retryVideoCallAsVoiceSpecified = retryVideoCallAsVoiceSpecified;
            addPhone.newPhone.services = services == null ? null : services.Data;
            addPhone.newPhone.servicesURL = servicesUrl;
            addPhone.newPhone.speeddials = speeddials == null ? null : speeddials.Data;
            addPhone.newPhone.traceFlag = traceFlag;
            addPhone.newPhone.traceFlagSpecified = traceFlagSpecified;
            addPhone.newPhone.userHoldMOHAudioSourceId = userHoldMOHAudioSourceId;

            try
            {
                response = client.addPhone(addPhone);
            }
            catch(System.Web.Services.Protocols.SoapException e)
            {
                IAxlSoap.ReportSoapError(e, log, ref code, ref message);

                return Result.fault.ToString();
            }
            catch(Exception e)
            {
                log.Write(TraceLevel.Error, Metreos.Utilities.Exceptions.FormatException(e));
                return IApp.VALUE_FAILURE;
            }

            return IApp.VALUE_SUCCESS;
        }

		public AddPhone()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.builtInBridgeStatus        = XStatus.Default;
            this.builtInBridgeStatusSpecified = false;
            this.callInfoPrivacyStatus      = XStatus.Default;
            this.callInfoPrivacyStatusSpecified = false;
            this.description                = null;
            this.deviceSecurityMode         = XDeviceSecurityMode.UseSystemDefault;
            this.deviceSecurityModeSpecified= false;
            this.enableExtensionMobility    = false;
            this.enableExtensionMobilitySpecified = false;
            this.associatedPC               = null;
            this.authenticationUrl          = null;
			this.@class						= XClass.Invalid;
            this.directoryUrl               = null;
            this.idleTimeout                = null;
			this.currentProfileId			= null;
			this.currentProfileName			= null;
            this.informationUrl             = null;
            this.idleUrl                    = null;
            this.phoneName                  = null;
			this.protocol					= XDeviceProtocol.Ciscostation;
			this.protocolSide				= XProtocolSide.User;
            this.callingSearchSpaceName     = null;
            this.callingSearchSpaceId       = null;
            this.devicePoolName             = null;
            this.devicePoolId               = null;
            this.locationName               = null;
            this.locationId                 = null;
            this.mediaResourceListName      = null;
            this.mediaResourceListId        = null;
            this.automatedAlternateRoutingCSSName   = null;
            this.automatedAlternateRoutingCSSId     = null;
            this.aarNeighborhoodName        = null;
            this.aarNeighborhoodId          = null;
			this.ownerUserId				= null;
            this.phoneTemplateName          = null;
            this.phoneTemplateId            = null;
            this.softkeyTemplateName        = null;
            this.softkeyTemplateId          = null;
            this.defaultProfileName         = null;
            this.defaultProfileId           = null;
            this.loadInfoSpecial            = false;
            this.loadInfoValue              = null;
            this.messageUrl                 = null;
            this.mlppDomainIdSpecified      = false;
            this.mlppDomainId               = 0;
            this.mlppIndicationStatusSpecified = false;
            this.mlppIndicationStatus       = XStatus.Default;
            this.networkHoldMOHAudioSourceId = null;
            this.preemption                 = XPreemption.Default;
            this.preemptionSpecified        = false;
            this.proxyServerUrl             = null;
            this.servicesUrl                = null;
            this.traceFlag                  = false;
            this.traceFlagSpecified         = false;
            this.userHoldMOHAudioSourceId   = null;
            this.services                   = null;
            this.speeddials                 = null;
            this.lines                      = null;
			this.product					= XProduct.Unknown;
			this.model						= XModel.Unknown;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new addPhoneResponse();
            this.message                    = String.Empty;
            this.username                   = IAxlSoap.DefaultCcmAdmin;
            this.code                       = 0;
        }

        public bool ValidateInput()
        {
            return true;
        }

        public enum Result
        {
            success,
            failure,
            fault,
        }  

        public XPhoneLines BuildLines()
        {
            if(lines == null || lines.Data == null)
            {
                return null;
            }
            if(lines.Data.Length == 0)
            {
                // no lines
				return new XPhoneLines();
            }

            XPhoneLines newLines = new XPhoneLines();
            newLines.Items = lines.Data;
            return newLines;
        }
	}
}
