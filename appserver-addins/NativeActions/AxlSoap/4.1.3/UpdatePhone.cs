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

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap333.Actions.UpdatePhone;

namespace Metreos.Native.AxlSoap413
{
	/// <summary> Wraps up the 'updatePhone' AXL SOAP method for Cisco CallManager 4.1.3 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap413.Globals.PACKAGE_DESCRIPTION)]
    public class UpdatePhone : INativeAction
	{
        [ActionParamField(Package.Params.BuiltInBridgeStatus.DISPLAY, Package.Params.BuiltInBridgeStatus.DESCRIPTION, false, Package.Params.BuiltInBridgeStatus.DEFAULT)]
        public XStatus BuiltInBridgeStatus { set { builtInBridgeStatus = value; } }
        
        [ActionParamField(Package.Params.BuiltInBridgeStatusSpecified.DISPLAY, Package.Params.BuiltInBridgeStatusSpecified.DESCRIPTION, false, Package.Params.BuiltInBridgeStatusSpecified.DEFAULT)]
        public bool BuiltInBridgeStatusSpecified { set { builtInBridgeStatusSpecified = value; } }

        [ActionParamField(Package.Params.CallInfoPrivacyStatus.DISPLAY, Package.Params.CallInfoPrivacyStatus.DESCRIPTION, false, Package.Params.CallInfoPrivacyStatus.DEFAULT)]
        public XStatus CallInfoPrivacyStatus { set { callInfoPrivacyStatus = value; } }

        [ActionParamField(Package.Params.CallInfoPrivacyStatusSpecified.DISPLAY, Package.Params.CallInfoPrivacyStatusSpecified.DESCRIPTION, false, Package.Params.CallInfoPrivacyStatusSpecified.DEFAULT)]
        public bool CallInfoPrivacyStatusSpecified { set { callInfoPrivacyStatusSpecified = value; } }

        [ActionParamField(Package.Params.Description.DISPLAY, Package.Params.Description.DESCRIPTION, false, Package.Params.Description.DEFAULT)]
        public string Description { set { description = value; } }

        [ActionParamField(Package.Params.DeviceSecurityMode.DISPLAY, Package.Params.DeviceSecurityMode.DESCRIPTION, false, Package.Params.DeviceSecurityMode.DEFAULT)]
        public XDeviceSecurityMode DeviceSecurityMode { set { deviceSecurityMode = value; } }

        [ActionParamField(Package.Params.DeviceSecurityModeSpecified.DISPLAY, Package.Params.DeviceSecurityModeSpecified.DESCRIPTION, false, Package.Params.DeviceSecurityModeSpecified.DEFAULT)]
        public bool DeviceSecurityModeSpecified { set { deviceSecurityModeSpecified = value; } }

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

        [ActionParamField(Package.Params.PhoneName.DISPLAY, Package.Params.PhoneName.DESCRIPTION, false, Package.Params.PhoneName.DEFAULT)]
        public string PhoneName { set { phoneName = value; } }

        [ActionParamField(Package.Params.PhoneId.DISPLAY, Package.Params.PhoneId.DESCRIPTION, false, Package.Params.PhoneId.DEFAULT)]
        public string PhoneId { set { phoneId = value; } }
        
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
        
        [ActionParamField(Package.Params.LoadInfoSpecial.DISPLAY, Package.Params.LoadInfoSpecial.DESCRIPTION, false, Package.Params.LoadInfoSpecial.DEFAULT)]
        public bool LoadInfoSpecial { set { loadInfoSpecial = value; } }
        
        [ActionParamField(Package.Params.LoadInfoValue.DISPLAY, Package.Params.LoadInfoValue.DESCRIPTION, false, Package.Params.LoadInfoValue.DEFAULT)]
        public string LoadInfoValue { set { loadInfoValue = value; } }

        [ActionParamField(Package.Params.MessageUrl.DISPLAY, Package.Params.MessageUrl.DESCRIPTION, false, Package.Params.MessageUrl.DEFAULT)]
        public string MessageUrl { set { messageUrl = value; } }

        [ActionParamField(Package.Params.MlppDomainId.DISPLAY, Package.Params.MlppDomainId.DESCRIPTION, false, Package.Params.MlppDomainId.DEFAULT)]
        public int MlppDomainId { set { mlppDomainId = value; } }

        [ActionParamField(Package.Params.MlppDomainIdSpecified.DISPLAY, Package.Params.MlppDomainIdSpecified.DESCRIPTION, false, Package.Params.MlppDomainIdSpecified.DEFAULT)]
        public bool MlppDomainIdSpecified { set { mlppDomainIdSpecified = value; } }

        [ActionParamField(Package.Params.MlppIndicationStatus.DISPLAY, Package.Params.MlppIndicationStatus.DESCRIPTION, false, Package.Params.MlppIndicationStatus.DEFAULT)]
        public XStatus MlppIndicationStatus { set { mlppIndicationStatus = value; } }

        [ActionParamField(Package.Params.MlppIndicationStatusSpecified.DISPLAY, Package.Params.MlppIndicationStatusSpecified.DESCRIPTION, false, Package.Params.MlppIndicationStatusSpecified.DEFAULT)]
        public bool MlppIndicationStatusSpecified { set { mlppIndicationStatusSpecified = value; } }

        [ActionParamField(Package.Params.NetworkHoldMOHAudioSourceId.DISPLAY, Package.Params.NetworkHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.NetworkHoldMOHAudioSourceId.DEFAULT)]
        public string NetworkHoldMOHAudioSourceId { set { networkHoldMOHAudioSourceId = value; } }

        [ActionParamField(Package.Params.NewName.DISPLAY, Package.Params.NewName.DESCRIPTION, false, Package.Params.NewName.DEFAULT)]
        public string NewName { set { newName = value; } }
        
        [ActionParamField(Package.Params.Preemption.DISPLAY, Package.Params.Preemption.DESCRIPTION, false, Package.Params.Preemption.DEFAULT)]
        public XPreemption Preemption { set { preemption = value; } }
       
        [ActionParamField(Package.Params.PreemptionSpecified.DISPLAY, Package.Params.PreemptionSpecified.DESCRIPTION, false, Package.Params.PreemptionSpecified.DEFAULT)]
        public bool PreemptionSpecified { set { preemptionSpecified = value; } }
       
        [ActionParamField(Package.Params.ProxyServerUrl.DISPLAY, Package.Params.ProxyServerUrl.DESCRIPTION, false, Package.Params.ProxyServerUrl.DEFAULT)]
        public string ProxyServerUrl { set { proxyServerUrl = value; } }
       
        [ActionParamField(Package.Params.RetryVideoCallAsVoice.DISPLAY, Package.Params.RetryVideoCallAsVoice.DESCRIPTION, false, Package.Params.RetryVideoCallAsVoice.DEFAULT)]
        public bool RetryVideoCallAsVoice { set { retryVideoCallAsVoice = value; } }
       
        [ActionParamField(Package.Params.RetryVideoCallAsVoiceSpecified.DISPLAY, Package.Params.RetryVideoCallAsVoiceSpecified.DESCRIPTION, false, Package.Params.RetryVideoCallAsVoiceSpecified.DEFAULT)]
        public bool RetryVideoCallAsVoiceSpecified { set { retryVideoCallAsVoiceSpecified = value; } }
       
        [ActionParamField(Package.Params.ServicesUrl.DISPLAY, Package.Params.ServicesUrl.DESCRIPTION, false, Package.Params.ServicesUrl.DEFAULT)]
        public string ServicesUrl { set { servicesUrl = value; } }
       
        [ActionParamField(Package.Params.TraceFlag.DISPLAY, Package.Params.TraceFlag.DESCRIPTION, false, Package.Params.TraceFlag.DEFAULT)]
        public bool TraceFlag { set { traceFlag = value; } }
       
        [ActionParamField(Package.Params.TraceFlagSpecified.DISPLAY, Package.Params.TraceFlagSpecified.DESCRIPTION, false, Package.Params.TraceFlagSpecified.DEFAULT)]
        public bool TraceFlagSpecified { set { traceFlagSpecified = value; } }
        
        [ActionParamField(Package.Params.UserHoldMOHAudioSourceId.DISPLAY, Package.Params.UserHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.UserHoldMOHAudioSourceId.DEFAULT)]
        public string UserHoldMOHAudioSourceId { set { userHoldMOHAudioSourceId = value; } }
        
        [ActionParamField(Package.Params.Speeddials.DISPLAY, Package.Params.Speeddials.DESCRIPTION, false, Package.Params.Speeddials.DEFAULT)]
        public Speeddials Speeddials { set { speeddials = value; } }
        
        [ActionParamField(Package.Params.Services.DISPLAY, Package.Params.Services.DESCRIPTION, false, Package.Params.Services.DEFAULT)]
        public Services Services { set { services = value; } }

        [ActionParamField(Package.Params.Lines.DISPLAY, Package.Params.Lines.DESCRIPTION, false, Package.Params.Lines.DEFAULT)]
        public Lines Lines { set { lines = value; } }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.UpdatePhoneResponse.DISPLAY, Package.Results.UpdatePhoneResponse.DESCRIPTION)]
        public updatePhoneResponse UpdatePhoneResponse { get { return response; } }

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
        private string directoryUrl;
        private string idleTimeout;
        private string idleUrl;
        private string informationUrl;
        private string phoneName;
        private string phoneId;
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
        private string phoneTemplateName;
        private string phoneTemplateId;
        private string softkeyTemplateName;
        private string softkeyTemplateId;
        private string defaultProfileName;
        private string defaultProfileId;
        private bool loadInfoSpecial;
        private string loadInfoValue;
        private string messageUrl;
        private int mlppDomainId;
        private bool mlppDomainIdSpecified;
        private XStatus mlppIndicationStatus;
        private bool mlppIndicationStatusSpecified;
        private string networkHoldMOHAudioSourceId;
        private string newName;
        private XPreemption preemption;
        private bool preemptionSpecified;
        private string proxyServerUrl;
        private bool retryVideoCallAsVoice;
        private bool retryVideoCallAsVoiceSpecified;
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
        private updatePhoneResponse response;

        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
            
            updatePhone updatePhone = new updatePhone();
            updatePhone.associatedPC = associatedPC;
            updatePhone.authenticationURL = authenticationUrl;
            updatePhone.builtInBridgeStatus = builtInBridgeStatus;
            updatePhone.builtInBridgeStatusSpecified = builtInBridgeStatusSpecified;
            updatePhone.callInfoPrivacyStatus = callInfoPrivacyStatus;
            updatePhone.callInfoPrivacyStatusSpecified = callInfoPrivacyStatusSpecified;
            updatePhone.description = description;
            updatePhone.deviceSecurityMode = deviceSecurityMode;
            updatePhone.deviceSecurityModeSpecified = deviceSecurityModeSpecified;
            updatePhone.directoryURL = directoryUrl;
            updatePhone.enableExtensionMobility = enableExtensionMobility;
            updatePhone.enableExtensionMobilitySpecified = enableExtensionMobilitySpecified;
            updatePhone.idleTimeout = idleTimeout;
            updatePhone.idleURL = idleUrl;
            updatePhone.informationURL = informationUrl;
            updatePhone.Item = IAxlSoap.DetermineChosenBetweenStrings(phoneName, phoneId);
            updatePhone.ItemElementName = (ItemChoiceType4) IAxlSoap.DetermineChosenBetweenStringsType(phoneName, phoneId, ItemChoiceType4.name, ItemChoiceType4.uuid);
            updatePhone.Item1 = IAxlSoap.DetermineChosenBetweenStrings(callingSearchSpaceName, callingSearchSpaceId);
            updatePhone.Item1ElementName = (Item1ChoiceType1) IAxlSoap.DetermineChosenBetweenStringsType(callingSearchSpaceName, callingSearchSpaceId,
                Item1ChoiceType1.callingSearchSpaceName, Item1ChoiceType1.callingSearchSpaceId);
            updatePhone.Item2 = IAxlSoap.DetermineChosenBetweenStrings(devicePoolName, devicePoolId);
            updatePhone.Item2ElementName = (Item2ChoiceType) IAxlSoap.DetermineChosenBetweenStringsType(devicePoolName, devicePoolId,
                Item2ChoiceType.devicePoolName, Item2ChoiceType.devicePoolId);
            updatePhone.Item3 = IAxlSoap.DetermineChosenBetweenStrings(locationName, locationId);
            updatePhone.Item3ElementName = (Item3ChoiceType) IAxlSoap.DetermineChosenBetweenStringsType(locationName, locationId, 
                Item3ChoiceType.locationName, Item3ChoiceType.locationId);
            updatePhone.Item4 = IAxlSoap.DetermineChosenBetweenStrings(mediaResourceListName, mediaResourceListId);
            updatePhone.Item4ElementName = (Item4ChoiceType) IAxlSoap.DetermineChosenBetweenStringsType(mediaResourceListName, mediaResourceListId,
                Item4ChoiceType.mediaResourceListName, Item4ChoiceType.mediaResourceListId);
            updatePhone.Item5 = IAxlSoap.DetermineChosenBetweenStrings(automatedAlternateRoutingCSSName, automatedAlternateRoutingCSSId);
            updatePhone.Item5ElementName = (Item5ChoiceType) IAxlSoap.DetermineChosenBetweenStringsType(
                automatedAlternateRoutingCSSName, automatedAlternateRoutingCSSId,
                Item5ChoiceType.automatedAlternateRoutingCSSName, Item5ChoiceType.automatedAlternateRoutingCSSId);
            updatePhone.Item6 = IAxlSoap.DetermineChosenBetweenStrings(aarNeighborhoodName, aarNeighborhoodId);
            updatePhone.Item6ElementName = (Item6ChoiceType) IAxlSoap.DetermineChosenBetweenStringsType(
                aarNeighborhoodName, aarNeighborhoodId,
                Item6ChoiceType.aarNeighborhoodName, Item6ChoiceType.aarNeighborhoodId);
            updatePhone.Item7 = IAxlSoap.DetermineChosenBetweenStrings(phoneTemplateName, phoneTemplateId); 
            updatePhone.Item7ElementName = (Item7ChoiceType) IAxlSoap.DetermineChosenBetweenStringsType(
                phoneTemplateName, phoneTemplateId, Item7ChoiceType.phoneTemplateName, Item7ChoiceType.phoneTemplateId);
            updatePhone.Item8 = IAxlSoap.DetermineChosenBetweenStrings(softkeyTemplateName, softkeyTemplateId);
            updatePhone.Item8ElementName = (Item8ChoiceType1) IAxlSoap.DetermineChosenBetweenStringsType(
                softkeyTemplateName, softkeyTemplateId, Item8ChoiceType1.softkeyTemplateName, Item8ChoiceType1.softkeyTemplateId);

            // Used in conjunction with Extension Mobility
            updatePhone.Item9 = IAxlSoap.DetermineChosenBetweenStrings(defaultProfileName, defaultProfileId); 
            updatePhone.Item9ElementName = (Item9ChoiceType) IAxlSoap.DetermineChosenBetweenStringsType(
                defaultProfileName, defaultProfileId, Item9ChoiceType.defaultProfileName, Item9ChoiceType.defaultProfileId);

            updatePhone.lines = BuildLines();

            XLoadInformation loadInfo = new XLoadInformation();
            loadInfo.special = loadInfoSpecial;
            loadInfo.specialSpecified = true;
            loadInfo.Value = loadInfoValue; // This will be a flag to say if its specified

            updatePhone.loadInformation = loadInfo.Value == null ? null : loadInfo;
            updatePhone.messagesURL = messageUrl;
            updatePhone.mlppDomainId = mlppDomainId;
            updatePhone.mlppDomainIdSpecified = mlppDomainIdSpecified;
            updatePhone.mlppIndicationStatus = mlppIndicationStatus;
            updatePhone.mlppIndicationStatusSpecified = mlppIndicationStatusSpecified;
            updatePhone.networkHoldMOHAudioSourceId = networkHoldMOHAudioSourceId;
            updatePhone.newName = newName;
            updatePhone.preemption = preemption;
            updatePhone.preemptionSpecified = preemptionSpecified;
            updatePhone.proxyServerURL = proxyServerUrl;
            updatePhone.retryVideoCallAsVoice = retryVideoCallAsVoice;
            updatePhone.retryVideoCallAsVoiceSpecified = retryVideoCallAsVoiceSpecified;
            updatePhone.services = services == null ? null : services.Data;
            updatePhone.servicesURL = servicesUrl;
            updatePhone.speeddials = speeddials == null ? null : speeddials.Data;
            updatePhone.traceFlag = traceFlag;
            updatePhone.traceFlagSpecified = traceFlagSpecified;
            updatePhone.userHoldMOHAudioSourceId = userHoldMOHAudioSourceId;

            try
            {
                response = client.updatePhone(updatePhone);
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

		public UpdatePhone()
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
            this.directoryUrl               = null;
            this.idleTimeout                = null;
            this.informationUrl             = null;
            this.idleUrl                    = null;
            this.phoneName                  = null;
            this.phoneId                    = null;
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
            this.MlppIndicationStatusSpecified = false;
            this.MlppIndicationStatus       = XStatus.Default;
            this.networkHoldMOHAudioSourceId = null;
            this.newName                    = null;
            this.preemption                 = XPreemption.Default;
            this.preemptionSpecified        = false;
            this.proxyServerUrl             = null;
            this.retryVideoCallAsVoiceSpecified  = false;
            this.retryVideoCallAsVoice      = false;
            this.servicesUrl                = null;
            this.traceFlag                  = false;
            this.traceFlagSpecified         = false;
            this.userHoldMOHAudioSourceId   = null;
            this.services                   = null;
            this.speeddials                 = null;
            this.lines                      = null;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new updatePhoneResponse();
            this.message                    = String.Empty;
            this.username                   = IAxlSoap.DefaultCcmAdmin;
            this.code                       = 0;
        }

        public bool ValidateInput()
        {
            if( (phoneName == null || phoneName == String.Empty) && 
                (phoneId == null || phoneId == String.Empty) )
            {
                log.Write(TraceLevel.Error, 
                    "Both 'PhoneName' and 'PhoneId' can not both be undefined.  " + 
                    "At least one must be defined");

                return false;
            }

            return true;
        }

        public enum Result
        {
            success,
            failure,
            fault,
        }  

        public UpdatePhoneReqLines BuildLines()
        {
            if(lines == null || lines.Data == null)
            {
                return null;
            }
            if(lines.Data.Length == 0)
            {
                // Indicates a 'clear lines' action. Will serialize as <lines />
                return new UpdatePhoneReqLines();
            }

            UpdatePhoneReqLines newLines = new UpdatePhoneReqLines();
            newLines.Items = lines.Data;
            return newLines;
        }
	}
}
