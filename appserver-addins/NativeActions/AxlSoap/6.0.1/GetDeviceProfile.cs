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

using Metreos.AxlSoap601;
using Metreos.AxlSoap;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap601.Actions.GetDeviceProfile;

namespace Metreos.Native.AxlSoap601
{
	/// <summary> Wraps up the 'getDeficeProfile' AXL SOAP method for Cisco CallManager 6.0.1 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap601.Globals.PACKAGE_DESCRIPTION)]
    public class GetDeviceProfile : INativeAction
	{
        [ActionParamField(Package.Params.ProfileName.DISPLAY, Package.Params.ProfileName.DESCRIPTION, false, Package.Params.ProfileName.DEFAULT)]
        public string ProfileName { set { profileName = value; } }

        [ActionParamField(Package.Params.ProfileId.DISPLAY, Package.Params.ProfileId.DESCRIPTION, false, Package.Params.ProfileId.DEFAULT)]
        public string ProfileId { set { profileId = value; } }
        
        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.GetDeviceProfileResponse.DISPLAY, Package.Results.GetDeviceProfileResponse.DESCRIPTION)]
        public getDeviceProfileResponse GetDeviceProfileResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private string profileName;
        private string profileId;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        private getDeviceProfileResponse response;

		public GetDeviceProfile()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.profileName    = null;
            this.profileId      = null;
            this.username       = IAxlSoap.DefaultCcmAdmin;
            this.password       = null;
            this.callManagerIP  = null;
            this.response       = new getDeviceProfileResponse();
            this.message        = String.Empty;
            this.code           = 0;
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

        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
            
            getDeviceProfile request = new getDeviceProfile();
            request.Item = IAxlSoap.DetermineChosenBetweenStrings(profileName, profileId);
            request.ItemElementName = (ItemChoiceType15)IAxlSoap.DetermineChosenBetweenStringsType(
                profileName, profileId, ItemChoiceType15.profileName, ItemChoiceType15.profileId);
            
            try
            {
                response = client.getDeviceProfile(request);
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
	}
}
