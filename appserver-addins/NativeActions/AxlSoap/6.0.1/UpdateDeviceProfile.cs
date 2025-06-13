using System;
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
using Metreos.AxlSoap601;
using Metreos.Types.AxlSoap601;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601.Actions.UpdateDeviceProfile;

namespace Metreos.Native.AxlSoap601
{
	/// <summary> Wraps up the 'updatePhone' AXL SOAP method for Cisco CallManager 6.0.1 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.PACKAGE_DESCRIPTION)]
    public class UpdateDeviceProfile : INativeAction
	{
		[ActionParamField(Package.Params.AllowCtiControlFlag.DISPLAY, Package.Params.AllowCtiControlFlag.DESCRIPTION, false, Package.Params.AllowCtiControlFlag.DEFAULT)]
		public bool AllowCtiControlFlag { set { allowCtiControlFlag = value; allowCtiControlFlagSpecified = true;} }

        [ActionParamField(Package.Params.Description.DISPLAY, Package.Params.Description.DESCRIPTION, false, Package.Params.Description.DEFAULT)]
        public string Description { set { description = value; } }

        [ActionParamField(Package.Params.ProfileName.DISPLAY, Package.Params.ProfileName.DESCRIPTION, false, Package.Params.ProfileName.DEFAULT)]
        public string ProfileName { set { profileName = value; } }

        [ActionParamField(Package.Params.ProfileId.DISPLAY, Package.Params.ProfileId.DESCRIPTION, false, Package.Params.ProfileId.DEFAULT)]
        public string ProfileId { set { profileId = value; } }
        
        [ActionParamField(Package.Params.PhoneTemplateName.DISPLAY, Package.Params.PhoneTemplateName.DESCRIPTION, false, Package.Params.PhoneTemplateName.DEFAULT)]
        public string PhoneTemplateName { set { phoneTemplateName = value; } }
        
        [ActionParamField(Package.Params.PhoneTemplateId.DISPLAY, Package.Params.PhoneTemplateId.DESCRIPTION, false, Package.Params.PhoneTemplateId.DEFAULT)]
        public string PhoneTemplateId { set { phoneTemplateId = value; } }
        
		[ActionParamField(Package.Params.CallingSearchSpaceName.DISPLAY, Package.Params.CallingSearchSpaceName.DESCRIPTION, false, Package.Params.CallingSearchSpaceName.DEFAULT)]
		public string CallingSearchSpaceName { set { callingSearchSpaceName = value; } }
        
		[ActionParamField(Package.Params.CallingSearchSpaceId.DISPLAY, Package.Params.CallingSearchSpaceId.DESCRIPTION, false, Package.Params.CallingSearchSpaceId.DEFAULT)]
		public string CallingSearchSpaceId { set { callingSearchSpaceId = value; } }

		[ActionParamField(Package.Params.PresenceGroupName.DISPLAY, Package.Params.PresenceGroupName.DESCRIPTION, false, Package.Params.PresenceGroupName.DEFAULT)]
		public string PresenceGroupName { set { presenceGroupName = value; } }
        
		[ActionParamField(Package.Params.PresenceGroupId.DISPLAY, Package.Params.PresenceGroupId.DESCRIPTION, false, Package.Params.PresenceGroupId.DEFAULT)]
		public string PresenceGroupId { set { presenceGroupId = value; } }

        [ActionParamField(Package.Params.SoftkeyTemplateName.DISPLAY, Package.Params.SoftkeyTemplateName.DESCRIPTION, false, Package.Params.SoftkeyTemplateName.DEFAULT)]
        public string SoftkeyTemplateName { set { softkeyTemplateName = value; } }
        
        [ActionParamField(Package.Params.SoftkeyTemplateId.DISPLAY, Package.Params.SoftkeyTemplateId.DESCRIPTION, false, Package.Params.SoftkeyTemplateId.DEFAULT)]
        public string SoftkeyTemplateId { set { softkeyTemplateId = value; } }
        
        [ActionParamField(Package.Params.NetworkHoldMOHAudioSourceId.DISPLAY, Package.Params.NetworkHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.NetworkHoldMOHAudioSourceId.DEFAULT)]
        public string NetworkHoldMOHAudioSourceId { set { networkHoldMOHAudioSourceId = value; } }

        [ActionParamField(Package.Params.NewName.DISPLAY, Package.Params.NewName.DESCRIPTION, false, Package.Params.NewName.DEFAULT)]
        public string NewName { set { newName = value; } }
        
        [ActionParamField(Package.Params.UserHoldMOHAudioSourceId.DISPLAY, Package.Params.UserHoldMOHAudioSourceId.DESCRIPTION, false, Package.Params.UserHoldMOHAudioSourceId.DEFAULT)]
        public string UserHoldMOHAudioSourceId { set { userHoldMOHAudioSourceId = value; } }
        
        [ActionParamField(Package.Params.Speeddials.DISPLAY, Package.Params.Speeddials.DESCRIPTION, false, Package.Params.Speeddials.DEFAULT)]
        public Speeddials Speeddials { set { speeddials = value; } }
        
        [ActionParamField(Package.Params.Services.DISPLAY, Package.Params.Services.DESCRIPTION, false, Package.Params.Services.DEFAULT)]
        public Services Services { set { services = value; } }

        [ActionParamField(Package.Params.Lines.DISPLAY, Package.Params.Lines.DESCRIPTION, false, Package.Params.Lines.DEFAULT)]
        public Lines Lines { set { lines = value; } }

		[ActionParamField(Package.Params.LoginUserId.DISPLAY, Package.Params.LoginUserId.DESCRIPTION, false, Package.Params.LoginUserId.DEFAULT)]
		public string LoginUserId { set { loginUserId = value; } }
		
		[ActionParamField(Package.Params.Preemption.DISPLAY, Package.Params.Preemption.DESCRIPTION, false, Package.Params.Preemption.DEFAULT)]
		public bool Preemption { set { preemption = value; /*preemptionSpecified = true;*/} }

		[ActionParamField(Package.Params.UserLocale.DISPLAY, Package.Params.UserLocale.DESCRIPTION, false, Package.Params.UserLocale.DEFAULT)]
		public string UserLocale { set { userLocale = value; /*userLocaleSpecified = true;*/} }

        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.UpdateDeviceProfileResponse.DISPLAY, Package.Results.UpdateDeviceProfileResponse.DESCRIPTION)]
        public updateDeviceProfileResponse UpdateDeviceProfileResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

		private bool allowCtiControlFlag;
		private bool allowCtiControlFlagSpecified;
        private string description;
        private string profileName;
        private string profileId;
        private string phoneTemplateName;
        private string phoneTemplateId;
		private string callingSearchSpaceName;
		private string callingSearchSpaceId;
		private string presenceGroupName;
		private string presenceGroupId;
        private string softkeyTemplateName;
        private string softkeyTemplateId;
        private string networkHoldMOHAudioSourceId;
        private string newName;
        private string userHoldMOHAudioSourceId;
        private Services services;
        private Speeddials speeddials;
        private Lines lines;
		private bool preemption;
		//private bool preemptionSpecified;
		private string userLocale;
		//private bool userLocaleSpecified;
		private string loginUserId;
        private string password;
        private string message;
        private string callManagerIP;
        private string username;
        private int code;
        private LogWriter log;
        private updateDeviceProfileResponse response;

        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
            
            updateDeviceProfile updateProfile = new updateDeviceProfile();
			// updateProfile.addOnModules; TODO
			updateProfile.allowCtiControlFlag = allowCtiControlFlag;
			updateProfile.allowCtiControlFlagSpecified = allowCtiControlFlagSpecified;
			//updateProfile.busyLampFields; TODO
			//updateProfile.ignorePresentationIndicators; TODO, looks to have default=false WSDL bug
            updateProfile.description = description;
            updateProfile.newName = newName;
            updateProfile.Item = IAxlSoap.DetermineChosenBetweenStrings(profileName, profileId);
            updateProfile.ItemElementName = (ItemChoiceType37) IAxlSoap.DetermineChosenBetweenStringsType(
                profileName, profileId, ItemChoiceType37.name, ItemChoiceType37.uuid);
            updateProfile.Item1 = IAxlSoap.DetermineChosenBetweenStrings(callingSearchSpaceName, callingSearchSpaceId);
            updateProfile.Item1ElementName = (Item1ChoiceType6)IAxlSoap.DetermineChosenBetweenStringsType(
                callingSearchSpaceName, callingSearchSpaceId, Item1ChoiceType6.subscribeCallingSearchSpaceName, Item1ChoiceType6.subscribeCallingSearchSpaceId);
            updateProfile.Item2 = IAxlSoap.DetermineChosenBetweenStrings(presenceGroupName, presenceGroupId);
            updateProfile.Item2ElementName = (Item2ChoiceType3)IAxlSoap.DetermineChosenBetweenStringsType(
                presenceGroupName, presenceGroupId, Item2ChoiceType3.PresenceGroupName, Item2ChoiceType3.presenceGroupID);
            updateProfile.Item3 = IAxlSoap.DetermineChosenBetweenStrings(phoneTemplateName, phoneTemplateId);
            updateProfile.Item3ElementName = (Item3ChoiceType2)IAxlSoap.DetermineChosenBetweenStringsType(
                phoneTemplateName, phoneTemplateId, Item3ChoiceType2.phoneTemplateName, Item3ChoiceType2.phoneTemplateId);
            updateProfile.Item4 = IAxlSoap.DetermineChosenBetweenStrings(softkeyTemplateName, softkeyTemplateId);
            updateProfile.Item4ElementName = (Item4ChoiceType1)IAxlSoap.DetermineChosenBetweenStringsType(
                softkeyTemplateName, softkeyTemplateId, Item4ChoiceType1.softkeyTemplateName, Item4ChoiceType1.softkeyTemplateId);
            updateProfile.loginUserId = loginUserId;
			
            //dpotter updateProfile.preemption = preemption;
			//dpotter updateProfile.preemptionSpecified = preemptionSpecified;
			updateProfile.userLocale	      = userLocale;
			
            //dpotter updateProfile.userLocaleSpecified = userLocaleSpecified;
			updateProfile.lines = BuildLines();
            updateProfile.services = services == null ? null : services.Data;
            updateProfile.speeddials = speeddials == null ? null : speeddials.Data;
            updateProfile.userHoldMOHAudioSourceId = userHoldMOHAudioSourceId;
            updateProfile.networkHoldMOHAudioSourceId = networkHoldMOHAudioSourceId;
            
            try
            {
                response = client.updateDeviceProfile(updateProfile);
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

		public UpdateDeviceProfile()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.allowCtiControlFlag        = false;//dpotter XBoolean.No;
			this.allowCtiControlFlagSpecified = false;
            this.description                = null;
            this.profileName                = null;
            this.profileId                  = null;
            this.phoneTemplateName          = null;
            this.phoneTemplateId            = null;
			this.callingSearchSpaceName     = null;
			this.callingSearchSpaceId		= null;
			this.presenceGroupName			= null;
			this.presenceGroupId			= null;
            this.softkeyTemplateName        = null;
            this.softkeyTemplateId          = null;
            this.networkHoldMOHAudioSourceId = null;
            this.newName                    = null;
            this.userHoldMOHAudioSourceId   = null;
            this.services                   = null;
            this.speeddials                 = null;
            this.lines                      = null;
            this.preemption                 = false;//dpotter XPreemption.Default;
			//this.preemptionSpecified		= false;
            this.userLocale                 = null;//dpotter string.EnglishUnitedStates;
			//this.userLocaleSpecified		= false;
			this.loginUserId				= null;
            this.password                   = null;
            this.callManagerIP              = null;
            this.response                   = new updateDeviceProfileResponse();
            this.message                    = String.Empty;
            this.username                   = IAxlSoap.DefaultCcmAdmin;
            this.code                       = 0;
        }

        public bool ValidateInput()
        {
            if( (profileName == null || profileName == String.Empty) && 
                (profileId == null || profileId == String.Empty) )
            {
                log.Write(TraceLevel.Error, 
                    "Both 'ProfileName' and 'ProfileId' can not both be undefined.  " + 
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

        public UpdateDeviceProfileReqLines BuildLines()
        {
            if(lines == null || lines.Data == null)
            {
                return null;
            }
            if(lines.Data.Length == 0)
            {
                // Indicates a 'clear lines' action. Will serialize as <lines />
                return new UpdateDeviceProfileReqLines();
            }

            UpdateDeviceProfileReqLines newLines = new UpdateDeviceProfileReqLines();
            newLines.Items = lines.Data;
            return newLines;
        }
	}
}
