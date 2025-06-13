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
using Metreos.AxlSoap504;

using Package = Metreos.Interfaces.PackageDefinitions.AxlSoap504.Actions.GetPhone;

namespace Metreos.Native.AxlSoap504
{
	/// <summary> Wraps up the 'getPhone' AXL SOAP method for Cisco CallManager 5.0.4 </summary>
    [PackageDecl(Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.NAMESPACE, Metreos.Interfaces.PackageDefinitions.AxlSoap504.Globals.PACKAGE_DESCRIPTION)]
    public class GetPhone : INativeAction
	{
        [ActionParamField(Package.Params.PhoneName.DISPLAY, Package.Params.PhoneName.DESCRIPTION, false, Package.Params.PhoneName.DEFAULT)]
        public string PhoneName { set { phoneName = value; } }

        [ActionParamField(Package.Params.PhoneId.DISPLAY, Package.Params.PhoneId.DESCRIPTION, false, Package.Params.PhoneId.DEFAULT)]
        public string PhoneId { set { phoneId = value; } }
        
        [ActionParamField(Package.Params.CallManagerIP.DISPLAY, Package.Params.CallManagerIP.DESCRIPTION, true, Package.Params.CallManagerIP.DEFAULT)]
        public string CallManagerIP { set { callManagerIP = value; } }

        [ActionParamField(Package.Params.AdminUsername.DISPLAY, Package.Params.AdminUsername.DESCRIPTION, false, Package.Params.AdminUsername.DEFAULT)]
        public string AdminUsername { set { username = value; } }

        [ActionParamField(Package.Params.AdminPassword.DISPLAY, Package.Params.AdminPassword.DESCRIPTION, true, Package.Params.AdminPassword.DEFAULT)]
        public string AdminPassword { set { password = value; } }

        [ResultDataField(Package.Results.GetPhoneResponse.DISPLAY, Package.Results.GetPhoneResponse.DESCRIPTION)]
        public getPhoneResponse GetPhoneResponse { get { return response; } }

        [ResultDataField(Package.Results.FaultMessage.DISPLAY, Package.Results.FaultMessage.DESCRIPTION)]
        public string FaultMessage { get { return message; } }

        [ResultDataField(Package.Results.FaultCode.DISPLAY, Package.Results.FaultCode.DESCRIPTION)]
        public int FaultCode { get { return code; } }

        public LogWriter Log { set { log = value; } }

        private string phoneName;
        private string phoneId;
        private string username;
        private string password;
        private string message;
        private string callManagerIP;
        private int code;
        private LogWriter log;
        private getPhoneResponse response;

		public GetPhone()
		{
		    Clear();	
		}

        public void Clear()
        {
            this.phoneName      = null;
            this.phoneId        = null;
            this.username       = IAxlSoap.DefaultCcmAdmin;
            this.password       = null;
            this.callManagerIP  = null;
            this.response       = new getPhoneResponse();
            this.message        = String.Empty;
            this.code           = 0;
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

        [ReturnValue(typeof(Result), "A 'failure' indicates a generic, unexpected error.  A 'fault' indicates a SOAP-specific error")]
        [Action(Package.NAME, false, Package.DISPLAY, Package.DESCRIPTION)]
        public string Execute(SessionData sessionData, IConfigUtility configUtility) 
        {
            AXLAPIService client = new AXLAPIService(callManagerIP, username, password);
            
            getPhone request = new getPhone();
            request.Item = IAxlSoap.DetermineChosenBetweenStrings(phoneName, phoneId);
            request.ItemElementName = (ItemChoiceType51) IAxlSoap.DetermineChosenBetweenStringsType(
                phoneName, phoneId, ItemChoiceType51.phoneName, ItemChoiceType51.phoneId);
            
            try
            {
                response = client.getPhone(request);
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
